# ネコ写真部の大図書館 - a good library for photography club and cats.
.NET Multi-Platform App UI(MAUI)とEntity Framework Coreの学習用として作成したフォトビューアーアプリケーションです。
Firebird Embdedd Serverをキャッシュとして利用します。

### License
This project was released under the MIT License.

### Build
```pwsh
cmake --build
```

### efcore migrations
```pwsh
dotnet tool restore
cd CommonLibrary
dotnet ef migrations add 'v0001'
```
