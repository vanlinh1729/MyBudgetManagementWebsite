using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBudgetManagement.Migrations
{
    /// <inheritdoc />
    public partial class Add_Migrate_Stored_Procedure : Migration
    {
        /// <inheritdoc />
       protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[CreateUser]
                                    @Id uniqueidentifier,
                                    @FirstName nvarchar(50),
                                    @LastName nvarchar(50),
                                    @Email nvarchar(100),
                                    @Password nvarchar(50)
                                AS
                                BEGIN
                                    INSERT INTO [dbo].[users] ([Id], [FirstName], [LastName], [Email], [Password])
                                    VALUES (@Id, @FirstName, @LastName, @Email, @Password);
                                    SELECT * FROM [dbo].[Users] WHERE [Id] = @Id;
                                END");

            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[GetUserByEmail]
                                    @Email nvarchar(100)
                                AS
                                BEGIN
                                    SELECT * FROM USERS WHERE Email = @Email;
                                END");

            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[LoginAccount]
                                    @Email nvarchar(100),
                                    @Password nvarchar(100)
                                AS
                                BEGIN
                                    SELECT * FROM USERS WHERE Email = @Email AND Password = @Password;
                                END");

            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[GetRoleByUserId]
                                    @userId uniqueidentifier
                                AS
                                BEGIN
                                    SELECT r.Id, r.Name FROM [dbo].[UserRoles] ur INNER JOIN [dbo].[Roles] r ON ur.[RoleId] = r.[Id] WHERE [UserId] = @userId;
                                END");

            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[ReadUser]
                                    @UserId uniqueidentifier
                                AS
                                BEGIN
                                    SELECT * FROM [dbo].[users] WHERE [Id] = @UserId;
                                END");

            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[UpdateUser]
                                    @UserId uniqueidentifier,
                                    @FirstName nvarchar(50),
                                    @LastName nvarchar(50),
                                    @Email nvarchar(100),
                                    @Password nvarchar(50)
                                AS
                                BEGIN
                                    UPDATE [dbo].[users]
                                    SET [FirstName] = @FirstName, [LastName] = @LastName, [Email] = @Email, [Password] = @Password
                                    WHERE [Id] = @UserId;
                                END");

            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[DeleteUser]
                                    @UserId uniqueidentifier
                                AS
                                BEGIN
                                    BEGIN TRANSACTION
                                    DELETE FROM [dbo].[transactions] WHERE [UserId] = @UserId;
                                    DELETE FROM [dbo].[userbalances] WHERE [UserId] = @UserId;
                                    DELETE FROM [dbo].[users] WHERE [Id] = @UserId;
                                    COMMIT TRANSACTION;
                                END");

            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[CreateUserBalance]
                                    @Id uniqueidentifier,
                                    @UserId uniqueidentifier,
                                    @Balance decimal(18, 2)
                                AS
                                BEGIN
                                    INSERT INTO [dbo].[userbalances] ([Id], [UserId], [Balance])
                                    VALUES (@Id, @UserId, @Balance);
                                END");

            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[ReadUserBalance]
                                    @UserId uniqueidentifier
                                AS
                                BEGIN
                                    SELECT * FROM [dbo].[userbalances] WHERE [UserId] = @UserId;
                                END");

            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[UpdateUserBalance]
                                    @UserId uniqueidentifier,
                                    @Balance decimal(18, 2)
                                AS
                                BEGIN
                                    UPDATE [dbo].[userbalances]
                                    SET [Balance] = @Balance
                                    WHERE [UserId] = @UserId;
                                END");

            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[CreateTransaction]
                                    @Id uniqueidentifier,
                                    @UserId uniqueidentifier,
                                    @Title nvarchar(100),
                                    @Description nvarchar(max),
                                    @Type tinyint,
                                    @Date datetime,
                                    @Amount decimal(18, 2)
                                AS
                                BEGIN
                                    INSERT INTO [dbo].[transactions] ([Id], [UserId], [Title], [Description], [Type], [Date], [Amount])
                                    VALUES (@Id, @UserId, @Title, @Description, @Type, @Date, @Amount);
                                    IF @Type = 0 -- income
                                    BEGIN
                                        UPDATE [dbo].[userbalances]
                                        SET [Balance] = [Balance] + @Amount
                                        WHERE [UserId] = @UserId;
                                    END
                                    ELSE IF @Type = 1 -- expense
                                    BEGIN
                                        UPDATE [dbo].[userbalances]
                                        SET [Balance] = [Balance] - @Amount
                                        WHERE [UserId] = @UserId;
                                    END
                                    SELECT * FROM Transactions WHERE Id = @Id;
                                END");

            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[UpdateTransaction]
                                    @TransactionId uniqueidentifier,
                                    @UserId uniqueidentifier,
                                    @Title nvarchar(100),
                                    @Description nvarchar(max),
                                    @Type tinyint,
                                    @Date datetime,
                                    @Amount decimal(18, 2)
                                AS
                                BEGIN
                                    DECLARE @OldAmount decimal(18, 2);
                                    DECLARE @OldType tinyint;
                                    SELECT @OldAmount = [Amount], @OldType = [Type] FROM [dbo].[transactions] WHERE [Id] = @TransactionId;
                                    UPDATE [dbo].[transactions]
                                    SET [Title] = @Title, [Description] = @Description, [Type] = @Type, [Date] = @Date, [Amount] = @Amount
                                    WHERE [Id] = @TransactionId;
                                    IF @OldType = 0 -- Income to Expense
                                    BEGIN
                                        IF @Type = 1 -- Expense
                                        BEGIN
                                            UPDATE [dbo].[userbalances]
                                            SET [Balance] = [Balance] - (@Amount + @OldAmount)
                                            WHERE [UserId] = @UserId;
                                        END
                                    END
                                    ELSE IF @OldType = 1 -- Expense to Income
                                    BEGIN
                                        IF @Type = 0 -- Income
                                        BEGIN
                                            UPDATE [dbo].[userbalances]
                                            SET [Balance] = [Balance] + (@Amount - @OldAmount)
                                            WHERE [UserId] = @UserId;
                                        END
                                    END
                                    ELSE -- Same Type
                                    BEGIN
                                        IF @Type = 0 -- Income
                                        BEGIN
                                            UPDATE [dbo].[userbalances]
                                            SET [Balance] = [Balance] + (@Amount - @OldAmount)
                                            WHERE [UserId] = @UserId;
                                        END
                                        ELSE IF @Type = 1 -- Expense
                                        BEGIN
                                            UPDATE [dbo].[userbalances]
                                            SET [Balance] = [Balance] - (@Amount - @OldAmount)
                                            WHERE [UserId] = @UserId;
                                        END
                                    END
                                END");

            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[DeleteTransaction]
                                    @TransactionId uniqueidentifier
                                AS
                                BEGIN
                                    DECLARE @UserId uniqueidentifier;
                                    DECLARE @Amount decimal(18, 2);
                                    DECLARE @Type tinyint;
                                    SELECT @UserId = [UserId], @Amount = [Amount], @Type = [Type] FROM [dbo].[transactions] WHERE [Id] = @TransactionId;
                                    DELETE FROM [dbo].[transactions] WHERE [Id] = @TransactionId;
                                    IF @Type = 0 -- income
                                    BEGIN
                                        UPDATE [dbo].[userbalances]
                                        SET [Balance] = [Balance] - @Amount
                                        WHERE [UserId] = @UserId;
                                    END
                                    ELSE IF @Type = 1 -- expense
                                    BEGIN
                                        UPDATE [dbo].[userbalances]
                                        SET [Balance] = [Balance] + @Amount
                                        WHERE [UserId] = @UserId;
                                    END
                                END");

            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[GetUserBalanceByUserId]
                                    @userId uniqueidentifier
                                AS
                                BEGIN
                                    SELECT * FROM UserBalances WHERE UserId = @userId;
                                END");

            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[SetRoleUserForNewAccount]
                                    @id uniqueidentifier,
                                    @userId uniqueidentifier,
                                    @roleId uniqueidentifier
                                AS
                                BEGIN
                                    Insert into [dbo].[UserRoles] ([Id], [UserId], [RoleId])
                                    values (@id,@userId, @roleId);
                                END");

            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[GetAllTransactionOfUser]
                                    @userid uniqueidentifier
                                AS
                                BEGIN
                                    Select * from Transactions where UserId = @userid;
                                END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Đảm bảo cung cấp các câu lệnh để xóa tất cả các thủ tục nếu cần thiết
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[CreateUser]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetUserByEmail]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[LoginAccount]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetRoleByUserId]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[ReadUser]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[UpdateUser]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[DeleteUser]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[CreateUserBalance]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[ReadUserBalance]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[UpdateUserBalance]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[CreateTransaction]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[UpdateTransaction]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[DeleteTransaction]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetUserBalanceByUserId]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[SetRoleUserForNewAccount]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[GetAllTransactionOfUser]");
        }
    }
}
