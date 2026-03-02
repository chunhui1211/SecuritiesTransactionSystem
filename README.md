使用框架：.NET8
使用語言：C#
---
題目背景（證券場景）
請實作一個「證券交易資料查詢系統」RESTful API，包含股票查詢、下單、委託查詢等
功能。
即時價https://mis.twse.com.tw/stock/api/getStockInfo.jsp?ex_ch=tse_2330.tw
---
**必做**
* 建立.NET 8 Web API專案（InMemory儲存）
* GET /api/v1/stocks?symbol=&keyword=
* GET /api/v1/stocks/{symbol}
* POST /api/v1/orders建立委託單（買/賣、價格、數量）
* GET /api/v1/orders/{id}
* Model驗證與錯誤處理（400/404）
* 啟用Swagger
* 分層架構
* 分頁查詢(股票列表)
* 單元測試
* 資料庫結構設計
* Log使用與設計
* Middleware or filter實作統一回傳格式
* 快取
* 驗證實作
* 壓力測試腳本撰寫
