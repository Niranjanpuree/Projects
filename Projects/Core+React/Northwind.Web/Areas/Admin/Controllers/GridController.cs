using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Northwind.Web.Infrastructure.Models;

namespace Northwind.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GridController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Index1()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            return PartialView();
        }

        [HttpGet]
        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add([FromBody] Dictionary<string, object> param)
        {
            if (ModelState.IsValid)
            {
                return Ok(new { status = true });
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        public IActionResult Edit([FromBody] Northwind.Core.Entities.Customer customer)
        {
            if (ModelState.IsValid)
            {
                return Ok(new { status = true });
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        public IActionResult ReportFields()
        {
            var fields = new List<GridviewField>();
            fields.Add(new GridviewField { FieldLabel = "Company Name", FieldName = "CompanyName", IsFilterable = true, IsSortable = true, OrderIndex = 1, IsDefaultSortField = true });
            fields.Add(new GridviewField { FieldLabel = "Contact Name", FieldName = "ContactName", IsFilterable = true, IsSortable = true, OrderIndex = 2, IsDefaultSortField = false });
            fields.Add(new GridviewField { FieldLabel = "Contact Title", FieldName = "ContactTitle", IsFilterable = true, IsSortable = true, OrderIndex = 3, IsDefaultSortField = false });
            fields.Add(new GridviewField { FieldLabel = "Country", FieldName = "Country", IsFilterable = true, IsSortable = true, OrderIndex = 4, IsDefaultSortField = false });
            fields.Add(new GridviewField { FieldLabel = "Phone", FieldName = "Phone", IsFilterable = false, IsSortable = false, OrderIndex = 5, IsDefaultSortField = false });

            return PartialView(fields);
        }

        [HttpGet]
        public IActionResult GridviewFields()
        {
            var fields = new List<GridviewField>();            
            fields.Add(new GridviewField { FieldLabel = "Company Name", FieldName = "CompanyName", IsFilterable = true, IsSortable = true, OrderIndex = 1, IsDefaultSortField = true });
            fields.Add(new GridviewField { FieldLabel = "Contact Name", FieldName = "ContactName", IsFilterable = true, IsSortable = true, OrderIndex = 2, IsDefaultSortField = false });
            fields.Add(new GridviewField { FieldLabel = "Contact Title", FieldName = "ContactTitle", IsFilterable = true, IsSortable = true, OrderIndex = 3, IsDefaultSortField = false });
            fields.Add(new GridviewField { FieldLabel = "Country", FieldName = "Country", IsFilterable = true, IsSortable = true, OrderIndex = 4, IsDefaultSortField = false });
            fields.Add(new GridviewField { FieldLabel = "Phone", FieldName = "Phone", IsFilterable = false, IsSortable = false, OrderIndex = 5, IsDefaultSortField = false });

            return Ok(fields);
        }

        [HttpGet]
        public IActionResult GetData(string searchValue)
        {
            string s = @"		{
			result: [{
            
            'CustomerID': 'ALFKI',
            'CompanyName': 'Alfreds Futterkiste',
            'ContactName': 'Maria Anders',
            'ContactTitle': 'Sales Representative',
            'Address': 'Obere Str. 57',
            'City': 'Berlin',
            'Region': null,
            'PostalCode': '12209',
            'Country': 'Germany',
            'Phone': '030-0074321',
            'Fax': '030-0076545',
            'Bool': null
            
        }, {
            
            'CustomerID': 'ANATR',
            'CompanyName': 'Ana Trujillo Emparedados y helados',
            'ContactName': 'Ana Trujillo',
            'ContactTitle': 'Owner',
            'Address': 'Avda. de la Constituci\u00f3n 2222',
            'City': 'M\u00e9xico D.F.',
            'Region': null,
            'PostalCode': '05021',
            'Country': 'Mexico',
            'Phone': '(5) 555-4729',
            'Fax': '(5) 555-3745',
            'Bool': null
            
        }, {
            
            'CustomerID': 'ANTON',
            'CompanyName': 'Antonio Moreno Taquer\u00eda',
            'ContactName': 'Antonio Moreno',
            'ContactTitle': 'Owner',
            'Address': 'Mataderos  2312',
            'City': 'M\u00e9xico D.F.',
            'Region': null,
            'PostalCode': '05023',
            'Country': 'Mexico',
            'Phone': '(5) 555-3932',
            'Fax': null,
            'Bool': true
            
        }, {
            
            'CustomerID': 'AROUT',
            'CompanyName': 'Around the Horn',
            'ContactName': 'Thomas Hardy',
            'ContactTitle': 'Sales Representative',
            'Address': '120 Hanover Sq.',
            'City': 'London',
            'Region': null,
            'PostalCode': 'WA1 1DP',
            'Country': 'UK',
            'Phone': '(171) 555-7788',
            'Fax': '(171) 555-6750',
            'Bool': null
            
        }, {
            
            'CustomerID': 'BERGS',
            'CompanyName': 'Berglunds snabbk\u00f6p',
            'ContactName': 'Christina Berglund',
            'ContactTitle': 'Order Administrator',
            'Address': 'Berguvsv\u00e4gen  8',
            'City': 'Lule\u00e5',
            'Region': null,
            'PostalCode': 'S-958 22',
            'Country': 'Sweden',
            'Phone': '0921-12 34 65',
            'Fax': '0921-12 34 67',
            'Bool': null,
            
        }, {
            
            'CustomerID': 'BLAUS',
            'CompanyName': 'Blauer See Delikatessen',
            'ContactName': 'Hanna Moos',
            'ContactTitle': 'Sales Representative',
            'Address': 'Forsterstr. 57',
            'City': 'Mannheim',
            'Region': null,
            'PostalCode': '68306',
            'Country': 'Germany',
            'Phone': '0621-08460',
            'Fax': '0621-08924',
            'Bool': null,
            
        }, {
            
            'CustomerID': 'BLONP',
            'CompanyName': 'Blondel p\u00e8re et fils',
            'ContactName': 'Fr\u00e9d\u00e9rique Citeaux',
            'ContactTitle': 'Marketing Manager',
            'Address': '24, place Kl\u00e9ber',
            'City': 'Strasbourg',
            'Region': null,
            'PostalCode': '67000',
            'Country': 'France',
            'Phone': '88.60.15.31',
            'Fax': '88.60.15.32',
            'Bool': null,
            
        }, {
           
            'CustomerID': 'BOLID',
            'CompanyName': 'B\u00f3lido Comidas preparadas',
            'ContactName': 'Mart\u00edn Sommer',
            'ContactTitle': 'Owner',
            'Address': 'C/ Araquil, 67',
            'City': 'Madrid',
            'Region': null,
            'PostalCode': '28023',
            'Country': 'Spain',
            'Phone': '(91) 555 22 82',
            'Fax': '(91) 555 91 99',
            'Bool': true,
            
        }, {
            
            'CustomerID': 'BONAP',
            'CompanyName': 'Bon app',
            'ContactName': 'Laurence Lebihan',
            'ContactTitle': 'Owner',
            'Address': '12, rue des Bouchers',
            'City': 'Marseille',
            'Region': null,
            'PostalCode': '13008',
            'Country': 'France',
            'Phone': '91.24.45.40',
            'Fax': '91.24.45.41',
            'Bool': null,
            
        }, {
            
            'CustomerID': 'BOTTM',
            'CompanyName': 'Bottom-Dollar Markets',
            'ContactName': 'Elizabeth Lincoln',
            'ContactTitle': 'Accounting Manager',
            'Address': '23 Tsawassen Blvd.',
            'City': 'Tsawassen',
            'Region': 'BC',
            'PostalCode': 'T2F 8M4',
            'Country': 'Canada',
            'Phone': '(604) 555-4729',
            'Fax': '(604) 555-3745',
            'Bool': null,
            
        }, {
            
            'CustomerID': 'BSBEV',
            'CompanyName': 'Bs Beverages',
            'ContactName': 'Victoria Ashworth',
            'ContactTitle': 'Sales Representative',
            'Address': 'Fauntleroy Circus',
            'City': 'London',
            'Region': null,
            'PostalCode': 'EC2 5NT',
            'Country': 'UK',
            'Phone': '(171) 555-1212',
            'Fax': null,
            'Bool': null,
            
        }, {
            
            'CustomerID': 'CACTU',
            'CompanyName': 'Cactus Comidas para llevar',
            'ContactName': 'Patricio Simpson',
            'ContactTitle': 'Sales Agent',
            'Address': 'Cerrito 333',
            'City': 'Buenos Aires',
            'Region': null,
            'PostalCode': '1010',
            'Country': 'Argentina',
            'Phone': '(1) 135-5555',
            'Fax': '(1) 135-4892',
            'Bool': null,
            
        }],
        'count': 91
    }";

            if(searchValue != null)
            {
                s = @"		{
			result: [{
            
            'CustomerID': 'ALFKI',
            'CompanyName': 'Alfreds Futterkiste',
            'ContactName': 'Maria Anders',
            'ContactTitle': 'Sales Representative',
            'Address': 'Obere Str. 57',
            'City': 'Berlin',
            'Region': null,
            'PostalCode': '12209',
            'Country': 'Germany',
            'Phone': '030-0074321',
            'Fax': '030-0076545',
            'Bool': null
            
        }, {
            
            'CustomerID': 'ANATR',
            'CompanyName': 'Ana Trujillo Emparedados y helados',
            'ContactName': 'Ana Trujillo',
            'ContactTitle': 'Owner',
            'Address': 'Avda. de la Constituci\u00f3n 2222',
            'City': 'M\u00e9xico D.F.',
            'Region': null,
            'PostalCode': '05021',
            'Country': 'Mexico',
            'Phone': '(5) 555-4729',
            'Fax': '(5) 555-3745',
            'Bool': null
            
        }],
        'count': 2}";
            }
            return Ok(Newtonsoft.Json.JsonConvert.DeserializeObject(s.Replace("\r\n","")));
        }
    }

}