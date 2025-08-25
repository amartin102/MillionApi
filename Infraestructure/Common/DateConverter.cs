using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Common
{
    public class DateConverter
    {
        public DateTime ConvertUtcToLocal(DateTime utcDateTime)
        {
            // Asegúrate de que el Kind sea Utc
            if (utcDateTime.Kind == DateTimeKind.Unspecified)
            {
                utcDateTime = DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc);
            }

            // Convierte de UTC a la zona horaria local del sistema
            return utcDateTime.ToLocalTime();
        }
    }
}
