# JPDict
JPDict is a Japanese dictionary for beginner on UWP (Universal Windows Platform).

## Build Status
[![Build Status](https://dev.azure.com/kevin0497/JPDict/_apis/build/status/gaojunxuan.JPDict?branchName=master)](https://dev.azure.com/kevin0497/JPDict/_build/latest?definitionId=1&branchName=master)

## Project files
This repository contains 4 projects.

### JapaneseDict (aka JapaneseDict.GUI)
The main application of JPDict, built with MVVM pattern (MVVM Light).

### JapaneseDict.QueryEngine
The query engine of JPDict. There're five databases in JPDict (dictionary db, update db, kanji db, kanji radical db and notebook db), and this project handles the database queries.
Also, this project contains a lemmatizer implemented using MeCab (http://taku910.github.io/mecab/).

### JapaneseDict.OnlineService
This project handles the communication with backend server.
You can find the source code for the backend at another repository named [JPDictBackend](https://github.com/gaojunxuan/JPDictBackend).

## Contributions
Feel free to post issues or create pull request.
