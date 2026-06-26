## 「今天吃什麼」餐廳抽選器
建立自己的餐廳資料庫，透過隨機抽選與喜好程度加權，協助解決每天不知道要吃什麼的選擇困難。

## 技術細節

本專案使用 **C# / .NET 8 WPF** 開發桌面應用程式，採用 MVVM 架構分離畫面、狀態與操作邏輯，讓 UI 顯示與餐廳資料處理保持清楚的責任邊界。

### 使用技術

- **.NET 8 WPF**：建立 Windows 桌面應用程式介面。
- **MVVM 架構**：以 ViewModel 管理畫面狀態與操作流程，降低 UI 與業務邏輯耦合。
- **Entity Framework Core**：負責餐廳資料的存取與資料庫操作。
- **SQLite**：作為本機資料庫，餐廳資料會儲存在使用者電腦中。
- **Dependency Injection**：透過 `Microsoft.Extensions.DependencyInjection` 管理 ViewModel、Service 與 DbContext 的建立。
- **RelayCommand**：封裝 WPF 指令綁定，讓按鈕操作可以直接對應到 ViewModel 行為。

### 主要功能

- 新增餐廳資料
- 查詢餐廳清單
- 修改餐廳名稱、喜好程度與營業時間
- 刪除餐廳資料
- 依目前時間抽選正在營業的餐廳
- 依餐廳喜好程度進行加權隨機抽選

### 抽選邏輯

抽選時會先讀取資料庫中的所有餐廳，並依目前時間過濾出「現在可以吃」的店家。

如果餐廳沒有設定營業時間，系統會視為全天候可抽選；如果有設定營業時間，則會比對當天營業區間，並支援跨日營業時段，例如 `22:00 ~ 02:00`。

完成營業狀態篩選後，系統會根據餐廳的喜好程度進行加權隨機抽選。喜好程度越高，被抽中的機率越高；最低也會保留基本權重，避免餐廳完全無法被抽中。

### 資料儲存

餐廳資料使用 SQLite 儲存在本機端，資料庫路徑位於使用者的 Local Application Data 目錄下：

```text
%LOCALAPPDATA%\JellyfishZero_WhatToEat\WhatToEat.db
```
應用程式啟動時會自動建立或更新資料庫結構，確保資料表與目前程式版本一致。

## 專案架構
WhatToEat

├── Commands        # WPF ICommand 實作

├── Data            # EF Core DbContext 與資料實體

├── Helper          # 表單與營業時間輔助邏輯

├── Models          # 餐廳資料操作服務

├── ViewModels      # MVVM 狀態與操作邏輯

├── Views           # WPF 視窗與 XAML 介面

└── Migrations      # EF Core 資料庫遷移紀錄

## 開發環境
- Visual Studio 2022 或以上
- .NET 8 SDK
- Windows 作業系統

## 畫面預覽

### 主畫面

<img width="778" height="443" alt="螢幕擷取畫面 2026-06-26 135734" src="https://github.com/user-attachments/assets/9d88420e-1f6e-45c9-ad57-fc7fb21853be" />

### 新增餐廳

<img width="785" height="536" alt="螢幕擷取畫面 2026-06-26 135918" src="https://github.com/user-attachments/assets/b3007209-92e2-4e46-a887-f275e931233b" />

### 修改餐廳

<img width="840" height="606" alt="螢幕擷取畫面 2026-06-26 135941" src="https://github.com/user-attachments/assets/c774daf6-2388-40b1-afd6-2e10e01a4244" />

### 刪除餐廳

<img width="502" height="269" alt="螢幕擷取畫面 2026-06-26 140026" src="https://github.com/user-attachments/assets/2a39beff-2f07-44a6-b9f9-493033011aad" />

### 查詢所有餐廳

<img width="881" height="536" alt="螢幕擷取畫面 2026-06-26 140007" src="https://github.com/user-attachments/assets/a5d3dc44-7da0-4d6f-b016-364034e60464" />
