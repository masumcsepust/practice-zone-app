using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace backend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Phase2_StreamingTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AudioRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StreamingSessionId = table.Column<int>(type: "integer", nullable: false),
                    FilePath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    FileSizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    RecordedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AudioRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StreamingSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    ConnectionId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StreamingSessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WebSocketConnections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ConnectionId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    IpAddress = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ConnectedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DisconnectedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebSocketConnections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpeechRecognitionResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StreamingSessionId = table.Column<int>(type: "integer", nullable: false),
                    RecognizedText = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    ResultType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    RecognizedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpeechRecognitionResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpeechRecognitionResults_StreamingSessions_StreamingSession~",
                        column: x => x.StreamingSessionId,
                        principalTable: "StreamingSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AudioRecords_StreamingSessionId",
                table: "AudioRecords",
                column: "StreamingSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_SpeechRecognitionResults_StreamingSessionId",
                table: "SpeechRecognitionResults",
                column: "StreamingSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_StreamingSessions_ConnectionId",
                table: "StreamingSessions",
                column: "ConnectionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StreamingSessions_UserId",
                table: "StreamingSessions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WebSocketConnections_ConnectionId",
                table: "WebSocketConnections",
                column: "ConnectionId");

            migrationBuilder.CreateIndex(
                name: "IX_WebSocketConnections_UserId",
                table: "WebSocketConnections",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AudioRecords");

            migrationBuilder.DropTable(
                name: "SpeechRecognitionResults");

            migrationBuilder.DropTable(
                name: "WebSocketConnections");

            migrationBuilder.DropTable(
                name: "StreamingSessions");
        }
    }
}
