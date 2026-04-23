# BookTracker 2

A cross-platform book tracking application built with .NET MAUI — a rebuild and expansion of a previous PHP/MySQL web project.

---

## About

BookTracker 2 allows you to track your personal book collection across Windows, Android, and iOS. Books can be added manually or looked up automatically by ISBN using the Google Books API. Authors and genres are managed automatically via find-or-create logic, so you never need to manually maintain separate lists.

---

## Current Features

- Add, edit, and delete books
- Author and genre management via find-or-create — type a name and it's created automatically if it doesn't exist yet
- Track reading status (Unread, Reading, Finished, Abandoned), format (Physical, Ebook, Audiobook), and rating (1–5)
- Search books by title and filter by status, format, author, and genre
- Google Books API integration — enter an ISBN to automatically populate title, author, and genre
- Local SQLite database — no account or internet connection required (except for ISBN lookup)

---

## Planned Features

- Reading progress and statistics page
- Star rating UI to replace the current dropdown
- Support for multiple authors and genres per book
- General UI polish and layout improvements

---

## Setup

This project requires a Google Books API key for ISBN lookup functionality.

1. Go to [Google Cloud Console](https://console.cloud.google.com)
2. Create a project and enable the **Books API**
3. Generate an API key under **Credentials** and restrict it to the Books API
4. Copy `Constants.example.cs` to `Constants.cs` in the project root
5. Replace `YOUR_API_KEY_HERE` with your actual API key

> `Constants.cs` is gitignored and will never be committed. Do not share your API key publicly.

ISBN lookup will fail silently without a valid API key — all other features work without one.

---

## Built With

- [.NET MAUI](https://learn.microsoft.com/en-us/dotnet/maui/) — cross-platform UI framework
- C# — application and business logic
- [SQLite](https://www.sqlite.org/) via [sqlite-net-pcl](https://github.com/praeclarum/sqlite-net) — local database
- [CommunityToolkit.Maui](https://github.com/CommunityToolkit/Maui) — additional MAUI controls and helpers
- [Google Books API](https://developers.google.com/books) — ISBN lookup

---

## Project Structure

The project follows a standard MVVM folder structure:

- **Data/** — Database and API service layer
- **Models/** — Data models and enums
- **ViewModels/** — MVVM ViewModels
- **Views/** — XAML pages and code-behind

---

## Notes

This project is being built as a learning exercise in .NET MAUI, MVVM architecture, SQLite integration, and external API consumption. It is a work in progress.
