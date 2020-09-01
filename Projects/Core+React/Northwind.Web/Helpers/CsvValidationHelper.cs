using CsvHelper;
using Microsoft.AspNetCore.Http;
using Northwind.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Northwind.Web.Models.ViewModels.EnumGlobal;

namespace Northwind.Web.Helpers
{
    public static class CsvValidationHelper
    {
        /// <summary>
        /// Checks whether file has the matching Header and also checks/reads the whole list of files.
        /// if throws exception the file is deleted
        /// </summary>
        public static IEnumerable<dynamic> ChecksValidHeaderAndReadTheFile(string path, string location, string previousFile, UploadMethodName methodName)
        {
            try
            {
                using (var reader = new StreamReader(path))
                using (var csv = new CsvReader(reader))
                {
                    switch (methodName)
                    {
                        case UploadMethodName.WorkBreakDownStructure:
                            var wbsHeader = csv.Configuration.RegisterClassMap<WorkBreakDownHeaderMap>();
                            var wbsRecord = csv.GetRecords<WorkBreakDownHeader>().ToList();
                            return wbsRecord;
                        case UploadMethodName.EmployeeBillingRate:
                            var ebrHeader = csv.Configuration.RegisterClassMap<EmployeeBillingRateHeaderMap>();
                            var ebrRecord = csv.GetRecords<EmployeeBillingRateHeader>().ToList();
                            return ebrRecord;
                        case UploadMethodName.SubcontractorLaborBillingRates:
                            var lbrHeader = csv.Configuration.RegisterClassMap<LaborCategoryRateHeaderMap>();
                            var lbrRecord = csv.GetRecords<LaborCategoryRateHeader>().ToList();
                            return lbrRecord;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                GetFile(location, previousFile);
                throw new ArgumentException("You uploaded a CSV that does not match the expected header. If you intended to upload a CSV file, please download the sample file and make edits to it and upload it again.");
            }
        }
        /// <summary>
        /// saves the CSV file to the folder
        /// </summary>
        public static bool SaveTheUpdatedCsv(List<dynamic> moduleType, string path)
        {
            var records = moduleType;
            using (var writer = new StreamWriter(path))
            using (var csv = new CsvWriter(writer))
            {
                csv.WriteRecords(records);
                return true;
            }
        }

        public static bool GetFile(string location, string previousFile)
        {
            var destinationPath = Path.Combine(location, "OldFiles");
            var locationPath = Path.Combine(location, previousFile);
            var sourceFile = Path.Combine(destinationPath, previousFile);
            var destinationFile = Path.Combine(location, previousFile);

            if (File.Exists(locationPath))
            {
                File.Delete(locationPath);
            }
            if (File.Exists(sourceFile))
            {
                System.IO.File.Move(sourceFile, destinationFile);
            }
            return true;
        }

        public static void MoveFile(string location, string previousFile)
        {
            if (!string.IsNullOrEmpty(previousFile))
            {
                var destinationPath = Path.Combine(location, "OldFiles");
                var sourceFile = Path.Combine(location, previousFile);
                var destinationFile = Path.Combine(destinationPath, previousFile);

                if (Directory.Exists(destinationPath))
                {
                    Directory.Delete(destinationPath, true);
                }
                Directory.CreateDirectory(destinationPath);
                if (File.Exists(sourceFile))
                {
                    System.IO.File.Move(sourceFile, destinationFile);
                }
            }
        }

        public static IEnumerable<dynamic> ReadFile(string path, UploadMethodName methodName)
        {
            try
            {
                using (var reader = new StreamReader(path))
                using (var csv = new CsvReader(reader))
                {
                    switch (methodName)
                    {
                        case UploadMethodName.WorkBreakDownStructure:
                            var wbsHeader = csv.Configuration.RegisterClassMap<WorkBreakDownHeaderMap>();
                            var wbsRecord = csv.GetRecords<WorkBreakDownHeader>().ToList();
                            return wbsRecord;
                        case UploadMethodName.EmployeeBillingRate:
                            var ebrHeader = csv.Configuration.RegisterClassMap<EmployeeBillingRateHeaderMap>();
                            var ebrRecord = csv.GetRecords<EmployeeBillingRateHeader>().ToList();
                            return ebrRecord;
                        case UploadMethodName.SubcontractorLaborBillingRates:
                            var lbrHeader = csv.Configuration.RegisterClassMap<LaborCategoryRateHeaderMap>();
                            var lbrRecord = csv.GetRecords<LaborCategoryRateHeader>().ToList();
                            return lbrRecord;
                    }
                    return null;
                }
            }
            catch (Exception)
            {
                throw new ArgumentException("You uploaded a CSV that does not match the expected header. If you intended to upload a CSV file, please download the sample file and make edits to it and upload it again.");
            }
        }
    }
}
