/* Copyright 2012 Marco Minerva, marco.minerva@gmail.com

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Globalization;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Foundation;
using System.Net.Http;

namespace JapaneseDict.OnlineService
{
    /// <summary>
    /// Specifies the audio format of the retrieved audio stream.
    /// </summary>
    public enum SpeakStreamFormat
    {
        /// <summary>
        /// Uses the WAVE file format.
        /// </summary>
        Wave,
        /// <summary>
        /// Uses the MP3 file format.
        /// </summary>
        MP3
    }

    /// <summary>
    /// Specifies the audio quality of the retrieved audio stream.
    /// </summary>
    public enum SpeakStreamQuality
    {
        /// <summary>
        /// Uses the max available quality.
        /// </summary>
        MaxQuality,
        /// <summary>
        /// Retrieves audio file with the minimum size.
        /// </summary>
        MinSize
    }

    /// <summary>
    /// The <strong>SpeechSynthesizer</strong> class provides methods to retrieve stream of audio file speaking text in various supported languages.
    /// </summary>
    /// <remarks>
    /// <para>To use this library, you need to go to <strong>Azure DataMarket</strong> at https://datamarket.azure.com/developer/applications and register your application. In this way, you'll obtain the <see cref="ClientID"/> and <see cref="ClientSecret"/> that are necessary to use <strong>Microsoft Translator Service</strong>.</para>
    /// <para>You also need to go to https://datamarket.azure.com/dataset/1899a118-d202-492c-aa16-ba21c33c06cb and subscribe the <strong>Microsoft Translator Service</strong>. There are many options, based on the amount of characters per month. The service is free up to 2 million characters per month.</para>
    /// </remarks>
    public sealed class SpeechSynthesizer
    {
        private const string BASE_URL = "http://api.microsofttranslator.com/v2/Http.svc/";
        private const string LANGUAGES_URI = "GetLanguagesForSpeak";
        private const string SPEAK_URI = "Speak?text={0}&language={1}&format={2}&options={3}";
        private const string TRANSLATE_URI = "Translate?text={0}&to={1}&contentType=text/plain";
        private const string DETECT_URI = "Detect?text={0}";
        private const string AUTHORIZATION_HEADER = "Authorization";
        private const int MAX_TEXT_LENGTH = 1000;
        private const int MAX_TEXT_LENGTH_FOR_AUTODETECTION = 100;

        //private DateTime tokenRequestTime;
        //private int tokenValiditySeconds;
        private string headerValue;

        #region Properties

        /// <summary>
        /// Gets or sets the Application Client ID that is necessary to use <strong>Microsoft Translator Service</strong>.
        /// </summary>
        /// <value>The Application Client ID.</value>
        /// <remarks>
        /// <para>Go to <strong>Azure DataMarket</strong> at https://datamarket.azure.com/developer/applications to register your application and obtain a Client ID.</para>
        /// <para>You also need to go to https://datamarket.azure.com/dataset/1899a118-d202-492c-aa16-ba21c33c06cb and subscribe the <strong>Microsoft Translator Service</strong>. There are many options, based on the amount of characters per month. The service is free up to 2 million characters per month.</para>
        /// </remarks>        
        public string ClientID { get; set; }

        /// <summary>
        /// Gets or sets the Application Client Secret that is necessary to use <strong>Microsoft Translator Service</strong>.
        /// </summary>
        /// <remarks>
        /// <value>The Application Client Secret.</value>
        /// <para>Go to <strong>Azure DataMaket</strong> at https://datamarket.azure.com/developer/applications to register your application and obtain a Client Secret.</para>
        /// <para>You also need to go to https://datamarket.azure.com/dataset/1899a118-d202-492c-aa16-ba21c33c06cb and subscribe the <strong>Microsoft Translator Service</strong>. There are many options, based on the amount of characters per month. The service is free up to 2 million characters per month.</para>
        /// </remarks>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets the string representing the supported language code to speak the text in.
        /// </summary>
        /// <value>The string representing the supported language code to speak the text in. The code must be present in the list of codes returned from the method <see cref="GetLanguagesAsync"/>.</value>
        /// <seealso cref="GetLanguagesAsync"/>
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets the audio format of the retrieved audio stream. Currently, <strong>Wav</strong> and <strong>MP3</strong> are supported.
        /// </summary>
        /// <value>The audio format of the retrieved audio stream. Currently, <strong>Wav</strong> and <strong>MP3</strong> are supported.</value>
        /// <remarks>The default value is <strong>Wave</strong>.</remarks>        
        public SpeakStreamFormat AudioFormat { get; set; }

        /// <summary>
        /// Gets or sets the audio quality of the retrieved audio stream. Currently, <strong>MaxQuality</strong> and <strong>MinSize</strong> are supported.
        /// </summary>
        /// <value>The audio quality of the retrieved audio stream. Currently, <strong>MaxQuality</strong> and <strong>MinSize</strong> are supported.</value>
        /// <remarks>
        /// With <strong>MaxQuality</strong>, you can get the voice with the highest quality, and with <strong>MinSize</strong>, you can get the voices with the smallest size. The default value is <strong>MinSize</strong>.
        /// </remarks>
        public SpeakStreamQuality AudioQuality { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the sentence to be spoken must be translated in the specified language.
        /// </summary>
        /// <value><strong>true</strong> if the sentence to be spoken must be translated in the specified language; otherwise, <strong>false</strong>.</value>
        /// <remarks>If you don't need to translate to text to be spoken, you can speed-up the the library setting the <strong>AutomaticTranslation</strong> property to <strong>false</strong>. In this way, the specified text is passed as is to the other methods, without performing any translation. The default value is <strong>true</strong>.</remarks>
        public bool AutomaticTranslation { get; set; }

        /// <summary>
        /// Gets the Mime type that corresponds to the selected <see cref="AudioFormat"/> value.
        /// </summary>
        /// <value>The Mime type that corresponds to the selected <see cref="AudioFormat"/> value.</value>
        /// <seealso cref="AudioFormat"/>
        public string MimeContentType
        {
            get
            {
                switch (AudioFormat)
                {
                    case SpeakStreamFormat.Wave:
                        return "audio/wav";

                    case SpeakStreamFormat.MP3:
                        return "audio/mp3";

                    default:
                        return string.Empty;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the language of the text must be automatically detected before text-to-speech.
        /// </summary>
        /// <value><strong>true</strong> if the language of the text must be automatically detected; otherwise, <strong>false</strong>.</value>
        /// <remarks><para>The <strong>AutoDetectLanguage</strong> property is used when the <see cref="GetSpeakBytesAsync(string, string)"/> method is invoked.
        /// It this case, if the <strong>AutoDetectLanguage</strong> property is set to <strong>true</strong>, the language of the text is auto-detected before speech stream request. Otherwise, the language specified in the <seealso cref="Language"/> property is used.</para>
        /// <para>If the language to use is explicitly specified, using the versions of the methods that accept it, no auto-detection is performed.</para>
        /// <para>The default value is <strong>true</strong>.</para>
        /// </remarks>
        /// <seealso cref="Language"/>
        public bool AutoDetectLanguage { get; set; }
        public AzureAuthToken AuthToken { get; set; }


        #endregion

        /// <summary>
        /// Initializes a new instance of the <strong>SpeechSynthesizer</strong> class, using the specified Client ID and Client Secret and the current system language.
        /// </summary>
        /// <param name="clientID">The Application Client ID.
        /// </param>
        /// <param name="clientSecret">The Application Client Secret.
        /// </param>
        /// <remarks><para>You must register your application on <strong>Azure DataMarket</strong> at https://datamarket.azure.com/developer/applications to obtain the Client ID and Client Secret needed to use the service.</para>
        /// <para>You also need to go to https://datamarket.azure.com/dataset/1899a118-d202-492c-aa16-ba21c33c06cb and subscribe the <strong>Microsoft Translator Service</strong>. There are many options, based on the amount of characters per month. The service is free up to 2 million characters per month.</para>
        /// </remarks>
        /// <seealso cref="ClientSecret"/>        
        /// <seealso cref="Language"/>
        public SpeechSynthesizer(string clientSecret)
            : this(clientSecret, CultureInfo.CurrentCulture.Name.ToLower())
        { }

        /// <summary>
        /// Initializes a new instance of the <strong>SpeechSynthesizer</strong> class, using the specified Client ID and Client Secret and the desired language.
        /// </summary>
        /// <param name="clientID">The Application Client ID.
        /// </param>
        /// <param name="clientSecret">The Application Client Secret.
        /// </param>
        /// <param name="language">A string representing the supported language code to speak the text in. The code must be present in the list of codes returned from the method <see cref="GetLanguagesAsync"/>.</param>
        /// <remarks><para>You must register your application on <strong>Azure DataMarket</strong> at https://datamarket.azure.com/developer/applications to obtain the Client ID and Client Secret needed to use the service.</para>
        /// <para>You also need to go to https://datamarket.azure.com/dataset/1899a118-d202-492c-aa16-ba21c33c06cb and subscribe the <strong>Microsoft Translator Service</strong>. There are many options, based on the amount of characters per month. The service is free up to 2 million characters per month.</para>
        /// </remarks>
        /// <seealso cref="ClientSecret"/>        
        /// <seealso cref="Language"/>
        public SpeechSynthesizer(string clientSecret, string language)
        {
            //ClientID = clientID;
            ClientSecret = clientSecret;
            Language = language;
            AudioFormat = SpeakStreamFormat.Wave;
            AudioQuality = SpeakStreamQuality.MinSize;
            AutomaticTranslation = true;
            AutoDetectLanguage = true;
            if (string.IsNullOrWhiteSpace(ClientSecret))
                throw new ArgumentException("Invalid Token");

            AuthToken = new AzureAuthToken(ClientSecret);
        }

        #region Get Languages

        /// <summary>
        /// Retrieves the languages available for speech synthesis.
        /// </summary>
        /// <returns>A string array containing the language codes supported for speech synthesis by <strong>Microsoft Translator Service</strong>.</returns>        
        /// <exception cref="ArgumentException">The <see cref="ClientID"/> or <see cref="ClientSecret"/> properties haven't been set.</exception>
        /// <remarks><para>This method performs a non-blocking request.</para>
        /// <para>For more information, go to http://msdn.microsoft.com/en-us/library/ff512415.aspx.
        /// </para>
        /// </remarks>    
        public IAsyncOperation<IEnumerable<string>> GetLanguagesAsync()
        {
            return this.GetLanguagesAsyncHelper().AsAsyncOperation();
        }

        private async Task<IEnumerable<string>> GetLanguagesAsyncHelper()
        {
            // Check if it is necessary to obtain/update access token.
            await this.UpdateToken();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BASE_URL);
            client.DefaultRequestHeaders.Add(AUTHORIZATION_HEADER, headerValue);
            using (Stream stream = await client.GetStreamAsync(LANGUAGES_URI))
            {
                DataContractSerializer dcs = new DataContractSerializer(typeof(string[]));
                string[] results = (string[])dcs.ReadObject(stream);

                return results.AsEnumerable();
            }
        }

        #endregion

        #region Translate

        /// <summary>
        /// Translates a text string into the language specified in the <seealso cref="Language"/> property.
        /// </summary>
        /// <returns>A string representing the translated text.</returns>
        /// <param name="text">A string representing the text to translate.</param>
        /// <exception cref="ArgumentException">
        /// <list type="bullet">
        /// <term>The <see cref="ClientID"/> or <see cref="ClientSecret"/> properties haven't been set.</term>
        /// <term>The <paramref name="text"/> parameter is longer than 1000 characters.</term>
        /// </list>
        /// </exception>
        /// <exception cref="ArgumentNullException">The <paramref name="text"/> parameter is <strong>null</strong> (<strong>Nothing</strong> in Visual Basic) or empty.</exception>
        /// <remarks><para>This method perform a non-blocking request for translation.</para>
        /// <para>For more information, go to http://msdn.microsoft.com/en-us/library/ff512421.aspx.
        /// </para>
        /// </remarks>
        /// <seealso cref="Language"/>   
        public IAsyncOperation<string> TranslateAsync(string text)
        {
            return this.TranslateAsyncHelper(text).AsAsyncOperation();
        }

        private async Task<string> TranslateAsyncHelper(string text)
        {
            return await this.TranslateAsync(text, Language);
        }

        /// <summary>
        /// Translates a text string into the specified language.
        /// </summary>
        /// <returns>A string representing the translated text.</returns>
        /// <param name="text">A string representing the text to translate.</param>
        /// <param name="to">A string representing the language code to translate the text into. The code must be present in the list of codes returned from the <see cref="GetLanguagesAsync"/> method.</param>
        /// <exception cref="ArgumentException">
        /// <list type="bullet">
        /// <term>The <see cref="ClientID"/> or <see cref="ClientSecret"/> properties haven't been set.</term>
        /// <term>The <paramref name="text"/> parameter is longer than 1000 characters.</term>
        /// </list>
        /// </exception>
        /// <exception cref="ArgumentNullException">The <paramref name="text"/> parameter is <strong>null</strong> (<strong>Nothing</strong> in Visual Basic) or empty.</exception>
        /// <remarks><para>This method perform a non-blocking request for translation.</para>
        /// <para>For more information, go to http://msdn.microsoft.com/en-us/library/ff512421.aspx.
        /// </para>
        /// </remarks>
        /// <seealso cref="Language"/>  
        public IAsyncOperation<string> TranslateAsync(string text, string to)
        {
            return this.TranslateAsyncHelper(text, to).AsAsyncOperation();
        }

        private async Task<string> TranslateAsyncHelper(string text, string to)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentNullException("text");

            if (text.Length > MAX_TEXT_LENGTH)
                throw new ArgumentException("text parameter cannot be longer than " + MAX_TEXT_LENGTH + " characters");

            // Check if it is necessary to obtain/update access token.
            await this.UpdateToken();

            if (string.IsNullOrEmpty(to))
                to = Language;

            string uri = string.Format(TRANSLATE_URI, Uri.EscapeDataString(text), to);

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BASE_URL);
            client.DefaultRequestHeaders.Add(AUTHORIZATION_HEADER, headerValue);

            using (Stream stream = await client.GetStreamAsync(uri))
            {
                DataContractSerializer dcs = new DataContractSerializer(typeof(string));
                string results = (string)dcs.ReadObject(stream);

                return results;
            }
        }

        #endregion

        #region Detect Language

        /// <summary>
        /// Detects the language of a text. 
        /// </summary>
        /// <param name="text">A string represeting the text whose language must be detected.</param>
        /// <returns>A string containing a two-character Language code for the given text.</returns>
        /// <exception cref="ArgumentException">
        /// <list type="bullet">
        /// <term>The <see cref="ClientID"/> or <see cref="ClientSecret"/> properties haven't been set.</term>
        /// <term>The <paramref name="text"/> parameter is longer than 1000 characters.</term>
        /// </list>
        /// </exception>
        /// <exception cref="ArgumentNullException">The <paramref name="text"/> parameter is <strong>null</strong> (<strong>Nothing</strong> in Visual Basic) or empty.</exception>
        /// <remarks><para>This method perform a non-blocking request for language code.</para>
        /// <para>For more information, go to http://msdn.microsoft.com/en-us/library/ff512427.aspx.
        /// </para></remarks>
        /// <seealso cref="GetLanguagesAsync"/>
        /// <seealso cref="Language"/> 
        public IAsyncOperation<string> DetectLanguageAsync(string text)
        {
            return this.DetectLanguageAsyncHelper(text).AsAsyncOperation();
        }

        private async Task<string> DetectLanguageAsyncHelper(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentNullException("text");

            text = text.Substring(0, Math.Min(text.Length, MAX_TEXT_LENGTH_FOR_AUTODETECTION));

            // Check if it is necessary to obtain/update access token.
            await this.UpdateToken();

            string uri = string.Format(DETECT_URI, Uri.EscapeDataString(text));

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BASE_URL);
            client.DefaultRequestHeaders.Add(AUTHORIZATION_HEADER, headerValue);

            using (Stream stream = await client.GetStreamAsync(uri))
            {
                DataContractSerializer dcs = new DataContractSerializer(typeof(string));
                string results = (string)dcs.ReadObject(stream);

                return results;
            }
        }

        #endregion

        #region Get Speak Bytes

        /// <summary>
        /// Returns a byte array containing a file speaking the passed-in text. If <see cref="AutoDetectLanguage"/> property is set to <strong>true</strong>, the <see cref="DetectLanguageAsync"/> method is used to detect the language of the speech stream. Otherwise, the language specified in the <see cref="Language"/> property is used.
        /// </summary>
        /// <param name="text">A string containing the sentence to be spoken.</param>
        /// <returns>A byte array containing a file speaking the passed-in text.</returns>
        /// <exception cref="ArgumentException">
        /// <list type="bullet">
        /// <term>The <see cref="ClientID"/> or <see cref="ClientSecret"/> properties haven't been set.</term>
        /// <term>The <paramref name="text"/> parameter is longer than 1000 characters.</term>
        /// </list>
        /// </exception>
        /// <exception cref="ArgumentNullException">The <paramref name="text"/> parameter is <strong>null</strong> (<strong>Nothing</strong> in Visual Basic) or empty.</exception>
        /// <remarks><para>This method perform a non-blocking request for the stream.</para>
        /// <para>For more information, go to http://msdn.microsoft.com/en-us/library/ff512420.aspx.
        /// </para></remarks>
        /// <see cref="AutoDetectLanguage"/>
        /// <see cref="DetectLanguageAsync"/>
        /// <seealso cref="Language"/>       
        public IAsyncOperation<IList<byte>> GetSpeakBytesAsync(string text)
        {
            return this.GetSpeakBytesAsyncHelper(text).AsAsyncOperation();
        }

        private async Task<IList<byte>> GetSpeakBytesAsyncHelper(string text)
        {
            return await this.GetSpeakBytesAsync(text, null);
        }

        /// <summary>
        /// Returns a byte array containing a file speaking the passed-in text in the desired language. If <paramref name="language"/> parameter is <strong>null</strong> (<strong>Nothing</strong> in Visual Basic) and the <see cref="AutoDetectLanguage"/> property is set to <strong>true</strong>, the <see cref="DetectLanguageAsync"/> method is used to detect the language of the speech stream. Otherwise, the language specified in the <see cref="Language"/> property is used.
        /// </summary>
        /// <param name="text">A string containing the sentence to be spoken.</param>
        /// <param name="language">A string representing the language code to speak the text in. The code must be present in the list of codes returned from the method <see cref="GetLanguagesAsync"/>.</param>
        /// <returns>A byte array containing a file speaking the passed-in text.</returns>
        /// <exception cref="ArgumentException">
        /// <list type="bullet">
        /// <term>The <see cref="ClientID"/> or <see cref="ClientSecret"/> properties haven't been set.</term>
        /// <term>The <paramref name="text"/> parameter is longer than 1000 characters.</term>
        /// </list>
        /// </exception>
        /// <exception cref="ArgumentNullException">The <paramref name="text"/> parameter is <strong>null</strong> (<strong>Nothing</strong> in Visual Basic) or empty.</exception>
        /// <remarks><para>This method perform a non-blocking request for the stream.</para>
        /// <para>For more information, go to http://msdn.microsoft.com/en-us/library/ff512420.aspx.
        /// </para></remarks>
        /// <seealso cref="Language"/>
        /// <seealso cref="GetLanguagesAsync"/>  
        public IAsyncOperation<IList<byte>> GetSpeakBytesAsync(string text, string language)
        {
            return this.GetSpeakBytesAsyncHelper(text, language).AsAsyncOperation();
        }

        private async Task<IList<byte>> GetSpeakBytesAsyncHelper(string text, string language)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentNullException("text");

            if (text.Length > MAX_TEXT_LENGTH)
                throw new ArgumentException("text parameter cannot be longer than " + MAX_TEXT_LENGTH + " characters");

            bool languageDetected = false;
            if (string.IsNullOrEmpty(language))
            {
                if (AutoDetectLanguage)
                {
                    language = await this.DetectLanguageAsync(text);
                    languageDetected = true;
                }
                else
                {
                    language = Language;
                }
            }

            if (AutomaticTranslation && !languageDetected)
                text = await this.TranslateAsync(text, language);

            // Check if it is necessary to obtain/update access token.
            await this.UpdateToken();

            string audioFormat = (AudioFormat == SpeakStreamFormat.Wave ? "audio/wav" : "audio/mp3");
            string audioQuality = (AudioQuality == SpeakStreamQuality.MinSize ? "MinSize" : "MaxQuality");
            string uri = string.Format(SPEAK_URI, Uri.EscapeDataString(text), language, Uri.EscapeDataString(audioFormat), audioQuality);

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BASE_URL);
            client.DefaultRequestHeaders.Add(AUTHORIZATION_HEADER, headerValue);

            var bytes = await client.GetByteArrayAsync(uri);
            return bytes.ToList();
        }

        #endregion

        #region Get Speak Stream

        /// <summary>
        /// Returns a <see cref="Windows.Storage.Streams.IRandomAccessStream">stream</see> of a file speaking the passed-in text. If <see cref="AutoDetectLanguage"/> property is set to <strong>true</strong>, the <see cref="DetectLanguageAsync"/> method is used to detect the language of the speech stream. Otherwise, the language specified in the <see cref="Language"/> property is used.
        /// </summary>
        /// <param name="text">A string containing the sentence to be spoken.</param>
        /// <returns>A <see cref="Windows.Storage.Streams.IRandomAccessStream">stream</see> object that contains a file speaking the passed-in text.</returns>
        /// <exception cref="ArgumentException">
        /// <list type="bullet">
        /// <term>The <see cref="ClientID"/> or <see cref="ClientSecret"/> properties haven't been set.</term>
        /// <term>The <paramref name="text"/> parameter is longer than 1000 characters.</term>
        /// </list>
        /// </exception>
        /// <exception cref="ArgumentNullException">The <paramref name="text"/> parameter is <strong>null</strong> (<strong>Nothing</strong> in Visual Basic) or empty.</exception>
        /// <remarks><para>This method perform a non-blocking request for the stream.</para>
        /// <para>For more information, go to http://msdn.microsoft.com/en-us/library/ff512420.aspx.
        /// </para></remarks>
        /// <see cref="AutoDetectLanguage"/>
        /// <see cref="DetectLanguageAsync"/>
        /// <seealso cref="Language"/>   
        public IAsyncOperation<IRandomAccessStream> GetSpeakStreamAsync(string text)
        {
            return this.GetSpeakStreamAsyncHelper(text).AsAsyncOperation();
        }

        private async Task<IRandomAccessStream> GetSpeakStreamAsyncHelper(string text)
        {
            return await this.GetSpeakStreamAsync(text, null);
        }

        /// <summary>
        /// Returns <see cref="Windows.Storage.Streams.IRandomAccessStream">stream</see> of a file speaking the passed-in text in the desired language. If <paramref name="language"/> parameter is <strong>null</strong> (<strong>Nothing</strong> in Visual Basic) and the <see cref="AutoDetectLanguage"/> property is set to <strong>true</strong>, the <see cref="DetectLanguageAsync"/> method is used to detect the language of the speech stream. Otherwise, the language specified in the <see cref="Language"/> property is used.
        /// </summary>
        /// <param name="text">A string containing the sentence to be spoken.</param>
        /// <param name="language">A string representing the language code to speak the text in. The code must be present in the list of codes returned from the method <see cref="GetLanguagesAsync"/>.</param>
        /// <returns>A <see cref="Windows.Storage.Streams.IRandomAccessStream">stream</see> object that contains a file speaking the passed-in text.</returns>
        /// <exception cref="ArgumentException">
        /// <list type="bullet">
        /// <term>The <see cref="ClientID"/> or <see cref="ClientSecret"/> properties haven't been set.</term>
        /// <term>The <paramref name="text"/> parameter is longer than 1000 characters.</term>
        /// </list>
        /// </exception>
        /// <exception cref="ArgumentNullException">The <paramref name="text"/> parameter is <strong>null</strong> (<strong>Nothing</strong> in Visual Basic) or empty.</exception>
        /// <remarks><para>This method perform a non-blocking request for the stream.</para>
        /// <para>For more information, go to http://msdn.microsoft.com/en-us/library/ff512420.aspx.
        /// </para></remarks>
        /// <seealso cref="Language"/>
        /// <seealso cref="GetLanguagesAsync"/> 
        public IAsyncOperation<IRandomAccessStream> GetSpeakStreamAsync(string text, string language)
        {
            return this.GetSpeakStreamAsyncHelper(text, language).AsAsyncOperation();
        }

        private async Task<IRandomAccessStream> GetSpeakStreamAsyncHelper(string text, string language)
        {
            var bytes = await this.GetSpeakBytesAsync(text, language);

            InMemoryRandomAccessStream ms = new InMemoryRandomAccessStream();
            DataWriter writer = new DataWriter(ms.GetOutputStreamAt(0));
            writer.WriteBytes(bytes.ToArray());
            await writer.StoreAsync();

            return ms;
        }

        #endregion

        private async Task UpdateToken()
        {
            //if (string.IsNullOrWhiteSpace(ClientID))
            //    throw new ArgumentException("Invalid Client ID. Go to Azure Marketplace and sign up for Microsoft Translator: https://datamarket.azure.com/developer/applications");

            this.headerValue= await AuthToken.GetAccessTokenAsync();
        }
    }
}
