using System;

namespace App.Mvc.ViewComponents.Alert.Models
{
    public class AlertViewComponentModel : IDisposable
    {
        public string Title { get; set; }
        public string Message { get; set; }

        public Types Type { get; set; }
        public enum Types
        {
            DEFAULT = 0,
            ERROR = 1,
            SUCCESS = 2,
            WARNING = 3,
            INFORMATION = 4
        };

        public Layouts Layout { get; set; }
        public enum Layouts
        {
            SIMPLE = 0,
            SWEET = 1
        };

        public bool Visible { get; set; }
        public bool Redirect { get; set; }

        public AlertViewComponentModel()
        {
            Visible = false;
            Redirect = false;
        }

        public string TypeCssName
        {
            get
            {
                if (Layout == Layouts.SIMPLE)
                {
                    switch (Type)
                    {
                        case Types.ERROR:
                            return "danger";
                        case Types.INFORMATION:
                            return "info";
                        case Types.SUCCESS:
                            return "success";
                        case Types.WARNING:
                            return "warning";
                        default:
                            return "light";
                    }

                }
                else if (Layout == Layouts.SWEET)
                {
                    switch (Type)
                    {
                        case Types.ERROR:
                            return "error";
                        case Types.INFORMATION:
                            return "info";
                        case Types.SUCCESS:
                            return "success";
                        case Types.WARNING:
                            return "warning";
                        default:
                            return "info";
                    }
                }

                return string.Empty;
            }
        }

        public void Dispose()
            => GC.SuppressFinalize(this);
    }
}
