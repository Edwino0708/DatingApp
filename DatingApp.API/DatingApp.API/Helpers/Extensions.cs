using DatingApp.API.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API
{
    public static class Extensions
    {
        MyProperty = 566;
        public static void AddAplicactionError(this HttpResponse response, string messange)
        {
            response.Headers.Add("Applicaction-Error", messange);
            response.Headers.Add("Access-Control-Expose-Heardes", "Application-Error");
            response.Headers.Add("Access-Control-Allows-Origin", "*");
        }

        public static void AddPagination(this HttpResponse response, int currentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);
            response.Headers.Add("Pagination", JsonConvert.SerializeObject(paginationHeader));
            response.Headers.Add("Access-Control-Expose-Heardes", "Pagination");

        }

        public static int CalculateAge(this DateTime theDataTime)
        {
            var age = DateTime.Today.Year - theDataTime.Year;
            if (theDataTime.AddYears(age) > DateTime.Today)
                age--;
            return age;
        }

        private class JsonConvert
        {
            internal static StringValues SerializeObject(PaginationHeader paginationHeader)
            {
                throw new NotImplementedException();
            }
        }
    }
}
