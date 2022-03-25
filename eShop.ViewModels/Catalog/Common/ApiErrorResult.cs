using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.ViewModels.Catalog.Common
{
    public class ApiErrorResult<t>:ApiResult<t>
    {
        public string[] VadilationErros { get; set; }

        public ApiErrorResult()
        {

        }

        public ApiErrorResult(string message)
        {
            IsSuccessed = false;
            Message = message;
        }

        public ApiErrorResult(string[] validationErros)
        {
            IsSuccessed = false;
            VadilationErros = validationErros;
        }
    }
}
