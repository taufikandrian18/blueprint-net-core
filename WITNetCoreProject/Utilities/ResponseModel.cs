using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WITNetCoreProject.Utilities {

    public class ResponseModel {

        public string EnumCode { get; set; }
        public Message Msg { get; set; }
        public object Data { get; set; }

        public int Count { get; set; }

        public static ResponseModel ResponseOk(object data) {

            var resp = new ResponseModel {

                EnumCode = "200",
                Msg = new Message {

                    En = "Success.",
                    Id = "Berhasil."
                },
                Data = data,
            };
            return resp;
        }

        public static ResponseModel ResponseOk(object data, int count) {

            var resp = new ResponseModel {

                EnumCode = "200",
                Msg = new Message {

                    En = "Success.",
                    Id = "Berhasil."
                },
                Data = data,
                Count = count,
            };
            return resp;
        }

        public static ResponseModel ResponseOk() {

            var resp = new ResponseModel {

                EnumCode = "200",
                Msg = new Message {

                    En = "Success.",
                    Id = "Berhasil."
                },
                Data = null,

            };
            return resp;
        }

        public static ResponseModel BadRequest(object data, Message msg) {

            var resp = new ResponseModel {

                EnumCode = "400",
                Msg = msg,
                Data = data
            };
            return resp;
        }

        public static ResponseModel UnauthorizedTokenRefresh(object data, Message msg)
        {

            var resp = new ResponseModel
            {

                EnumCode = "401",
                Msg = msg,
                Data = data
            };
            return resp;
        }

        public static ResponseModel BadRequest(object data) {

            var resp = new ResponseModel {

                EnumCode = "400",
                Msg = new Message {

                    En = "Invalid data format.",
                    Id = "Data tidak valid."
                },
                Data = data
            };
            return resp;
        }

        public static ResponseModel Created(object data) {

            var resp = new ResponseModel {

                EnumCode = "201",
                Msg = new Message {

                    En = "Data has been created.",
                    Id = "Data telah berhasil ditambahkan."
                },
                Data = data
            };
            return resp;
        }

        public static ResponseModel Error(object data, Exception ex) {

            var resp = new ResponseModel {

                EnumCode = "500",
                Msg = new Message {

                    En = "Internal server error.",
                    Id = "Terjadi kesalahan pada server."
                },
                Data = ex.Message
            };
            return resp;
        }
    }

    public class Message {

        public string En { get; set; }
        public string Id { get; set; }
    }
}
