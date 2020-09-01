﻿using System.Globalization;
using System.Text;
using NLog;
using NLog.Config;
using NLog.LayoutRenderers;

namespace AutoCare.Product.Application.Infrastructure
{
    [LayoutRenderer("utc_date")]
    public class UtcDateLayoutRenderer : LayoutRenderer
    {
        ///
        /// Initializes a new instance of the  class.
        ///
        public UtcDateLayoutRenderer()
        {
            this.Format = "G";
            this.Culture = CultureInfo.InvariantCulture;
        }
        ///
        /// Gets or sets the culture used for rendering.
        ///
        ///
        public CultureInfo Culture { get; set; }

        ///
        /// Gets or sets the date format. Can be any argument accepted by DateTime.ToString(format).
        ///
        ///
        [DefaultParameter]
        public string Format { get; set; }

        ///
        /// Renders the current date and appends it to the specified .
        ///
        /// <param name="builder">The  to append the rendered data to.
        /// <param name="logEvent">Logging event.
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            builder.Append(logEvent.TimeStamp.ToUniversalTime().ToString(this.Format, this.Culture));
        }

    }
}
