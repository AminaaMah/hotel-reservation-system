CREATE TABLE IF NOT EXISTS Users (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    FirstName TEXT NOT NULL,
    LastName TEXT NOT NULL,
    Email TEXT NOT NULL UNIQUE COLLATE NOCASE,
    Phone TEXT NOT NULL,
    PasswordHash TEXT NOT NULL,
    CreatedAt TEXT NOT NULL DEFAULT (datetime('now'))
);

CREATE TABLE IF NOT EXISTS Admins (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Username TEXT NOT NULL UNIQUE COLLATE NOCASE,
    PasswordHash TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS Reservations (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    UserId INTEGER NOT NULL REFERENCES Users (Id) ON DELETE CASCADE,
    CheckIn TEXT NOT NULL,
    CheckOut TEXT NOT NULL,
    Adults INTEGER NOT NULL CHECK (Adults >= 1),
    Children INTEGER NOT NULL DEFAULT 0 CHECK (Children >= 0),
    RoomCode TEXT NOT NULL,
    BoardCode TEXT NOT NULL,
    PaymentMethod TEXT NOT NULL CHECK (PaymentMethod IN ('Card', 'PayAtHotel')),
    PaymentStatus TEXT NOT NULL CHECK (PaymentStatus IN ('Paid', 'Unpaid')),
    CardLast4 TEXT,
    TotalPrice NUMERIC NOT NULL CHECK (TotalPrice >= 0),
    Status TEXT NOT NULL DEFAULT 'Active' CHECK (Status IN ('Active', 'Cancelled')),
    CreatedAt TEXT NOT NULL DEFAULT (datetime('now')),
    CHECK (CheckOut > CheckIn)
);

CREATE INDEX IF NOT EXISTS IX_Reservations_Room_Dates
    ON Reservations (RoomCode, Status, CheckIn, CheckOut);

CREATE INDEX IF NOT EXISTS IX_Reservations_UserId
    ON Reservations (UserId);
