using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Departments_DepartmentDeptId",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "DepartmentDeptId",
                table: "Students",
                newName: "DeptId");

            migrationBuilder.RenameIndex(
                name: "IX_Students_DepartmentDeptId",
                table: "Students",
                newName: "IX_Students_DeptId");

            migrationBuilder.AddColumn<double>(
                name: "Degree",
                table: "Enrollments",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MinDegree",
                table: "Courses",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Departments_DeptId",
                table: "Students",
                column: "DeptId",
                principalTable: "Departments",
                principalColumn: "DeptId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Departments_DeptId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Degree",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "MinDegree",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "DeptId",
                table: "Students",
                newName: "DepartmentDeptId");

            migrationBuilder.RenameIndex(
                name: "IX_Students_DeptId",
                table: "Students",
                newName: "IX_Students_DepartmentDeptId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Departments_DepartmentDeptId",
                table: "Students",
                column: "DepartmentDeptId",
                principalTable: "Departments",
                principalColumn: "DeptId");
        }
    }
}
