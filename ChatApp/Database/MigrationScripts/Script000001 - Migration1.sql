START TRANSACTION;

CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

CREATE TABLE "Roles" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "Name" text NOT NULL,
    CONSTRAINT "PK_Roles" PRIMARY KEY ("Id")
);

CREATE TABLE "Users" (
    "Id" uuid NOT NULL,
    "Email" text NOT NULL,
    "IdentityProviderUId" text NOT NULL,
    CONSTRAINT "PK_Users" PRIMARY KEY ("Id")
);

CREATE TABLE "RefreshTokens" (
    "Id" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "RefreshTokenHash" bytea NOT NULL,
    "ExpiresOnUtc" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_RefreshTokens" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_RefreshTokens_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE TABLE "RoleUser" (
    "RolesId" integer NOT NULL,
    "UsersId" uuid NOT NULL,
    CONSTRAINT "PK_RoleUser" PRIMARY KEY ("RolesId", "UsersId"),
    CONSTRAINT "FK_RoleUser_Roles_RolesId" FOREIGN KEY ("RolesId") REFERENCES "Roles" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_RoleUser_Users_UsersId" FOREIGN KEY ("UsersId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

INSERT INTO "Roles" ("Id", "Name")
VALUES (1, 'User');
INSERT INTO "Roles" ("Id", "Name")
VALUES (2, 'Admin');

CREATE INDEX "IX_RefreshTokens_UserId" ON "RefreshTokens" ("UserId");

CREATE INDEX "IX_RoleUser_UsersId" ON "RoleUser" ("UsersId");

SELECT setval(
    pg_get_serial_sequence('"Roles"', 'Id'),
    GREATEST(
        (SELECT MAX("Id") FROM "Roles") + 1,
        nextval(pg_get_serial_sequence('"Roles"', 'Id'))),
    false);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250326110724_Migration1', '8.0.13');

COMMIT;