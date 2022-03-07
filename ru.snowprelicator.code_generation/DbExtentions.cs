using Microsoft.EntityFrameworkCore.Metadata;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System.Linq;

namespace ru.snowprelicator.code_generation
{

    //
    // неопознанный класс непонятного предназначения
    //
    // при его отсутсвии идут ошибки:
    // (3,7): error CS0246: Не удалось найти тип или имя пространства имен "Npgsql" (возможно, отсутствует директива using или ссылка на сборку).
    // (10,47): error CS0246: Не удалось найти тип или имя пространства имен "DbContext" (возможно, отсутствует директива using или ссылка на сборку).
    // (21,24): error CS0246: Не удалось найти тип или имя пространства имен "DbSet<>" (возможно, отсутствует директива using или ссылка на сборку).
    // (23,47): error CS0246: Не удалось найти тип или имя пространства имен "DbContextOptionsBuilder" (возможно, отсутствует директива using или ссылка на сборку).
    // (31,49): error CS0246: Не удалось найти тип или имя пространства имен "ModelBuilder" (возможно, отсутствует директива using или ссылка на сборку).
    // (46,45): error CS0246: Не удалось найти тип или имя пространства имен "ModelBuilder" (возможно, отсутствует директива using или ссылка на сборку).
    // (16,35): error CS0246: Не удалось найти тип или имя пространства имен "DbContextOptions<>" (возможно, отсутствует директива using или ссылка на сборку).
    // (17,15): error CS1729: "DbContext" не содержит конструктор, который принимает аргументы 1.

    public static class DbExtentions
    {
        public static bool IsPrimary(this IColumn column)
        {
            return column.Table.PrimaryKey?.Columns.Any(c => column.Name.Equals(c.Name)) ?? false;
        }

        public static bool IsSerial(this IColumn column)
        {
            return column.GetAnnotations()
                .Select(it => it.Value)
                .OfType<NpgsqlValueGenerationStrategy>()
                .Contains(NpgsqlValueGenerationStrategy.SerialColumn);
        }
    }
}
