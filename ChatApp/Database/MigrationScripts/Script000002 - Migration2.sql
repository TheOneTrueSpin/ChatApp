START TRANSACTION;

CREATE TABLE "Chats" (
    "Id" uuid NOT NULL,
    CONSTRAINT "PK_Chats" PRIMARY KEY ("Id")
);

CREATE TABLE "ChatUser" (
    "ChatsId" uuid NOT NULL,
    "ParticipantsId" uuid NOT NULL,
    CONSTRAINT "PK_ChatUser" PRIMARY KEY ("ChatsId", "ParticipantsId"),
    CONSTRAINT "FK_ChatUser_Chats_ChatsId" FOREIGN KEY ("ChatsId") REFERENCES "Chats" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_ChatUser_Users_ParticipantsId" FOREIGN KEY ("ParticipantsId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Messages" (
    "Id" uuid NOT NULL,
    "SentOnUTC" timestamp with time zone NOT NULL,
    "SenderId" uuid NOT NULL,
    "MessageContents" text NOT NULL,
    "ChatId" uuid,
    CONSTRAINT "PK_Messages" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Messages_Chats_ChatId" FOREIGN KEY ("ChatId") REFERENCES "Chats" ("Id"),
    CONSTRAINT "FK_Messages_Users_SenderId" FOREIGN KEY ("SenderId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_ChatUser_ParticipantsId" ON "ChatUser" ("ParticipantsId");

CREATE INDEX "IX_Messages_ChatId" ON "Messages" ("ChatId");

CREATE INDEX "IX_Messages_SenderId" ON "Messages" ("SenderId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250424124438_Migration2', '8.0.13');

COMMIT;