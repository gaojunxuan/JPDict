# JPDict
JPDict is a Japanese dictionary for beginner on UWP (Universal Windows Platform).

## Project files
This repository contains 4 projects.

### JapaneseDict (aka JapaneseDict.GUI)
The main application of JPDict, built with MVVM pattern (MVVM Light).

### JapaneseDict.QueryEngine
The query engine of JPDict. There're five databases in JPDict (dictionary db, update db, kanji db, kanji radical db and notebook db), and this project handles the database queries.
Also, in this project, we implemented a lemmatizer using MeCab (http://taku910.github.io/mecab/).

### JapaneseDict.OnlineService
This project handles the communication with backend server.
You can check out the source code for backend at another repository called JPDictBackend.

### JapaneseDict.Util
Utility project

## Contributions
Feel free to post issues or create pull request.
