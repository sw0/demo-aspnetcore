using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiDemo.Common
{
    public class ServiceResponse
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }

        public ServiceResponse With(bool success, string code, string msg)
        {
            this.Success = success;
            this.Code = code;
            this.Message = msg;
            return this;
        }

        public ServiceResponse Ok()
        {
            Success = true;
            return this;
        }

        public ServiceResponse Fail()
        {
            Success = false;
            return this;
        }

        public ServiceResponse WithMessage(string msg)
        {
            this.Message = msg;
            return this;
        }

        public ServiceResponse WithCode(string code)
        {
            this.Code = code;
            return this;
        }
    }

    public class ServiceResponse<T> : ServiceResponse
    {
        public T Data { get; set; }

        public ServiceResponse<T> WithData(T data)
        {
            Data = data;

            return this;
        }

        public new ServiceResponse<T> With(bool success, string code, string msg)
        {
            this.Success = success;
            this.Code = code;
            this.Message = msg;
            return this;
        }

        public new ServiceResponse<T> Ok()
        {
            Success = true;
            return this;
        }

        public new ServiceResponse<T> Fail()
        {
            Success = false;
            return this;
        }

        public new ServiceResponse<T> WithMessage(string msg)
        {
            base.WithMessage(msg);
            return this;
        }

        public new ServiceResponse<T> WithCode(string code)
        {
            base.WithCode(code);
            return this;
        }
    }
}
