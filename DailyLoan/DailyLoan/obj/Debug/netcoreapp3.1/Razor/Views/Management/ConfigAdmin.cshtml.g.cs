#pragma checksum "C:\Users\golf_\Documents\GitHub\DailyLoan\DailyLoan\Views\Management\ConfigAdmin.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "dd88bc1f7d6a1291c57a2fb981731c78ef6ab4f8"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Management_ConfigAdmin), @"mvc.1.0.view", @"/Views/Management/ConfigAdmin.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\golf_\Documents\GitHub\DailyLoan\DailyLoan\Views\_ViewImports.cshtml"
using DailyLoan;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\golf_\Documents\GitHub\DailyLoan\DailyLoan\Views\_ViewImports.cshtml"
using DailyLoan.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "C:\Users\golf_\Documents\GitHub\DailyLoan\DailyLoan\Views\Management\ConfigAdmin.cshtml"
using DailyLoan.Library;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\golf_\Documents\GitHub\DailyLoan\DailyLoan\Views\Management\ConfigAdmin.cshtml"
using DailyLoan.Library.Status;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"dd88bc1f7d6a1291c57a2fb981731c78ef6ab4f8", @"/Views/Management/ConfigAdmin.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ce9f4d3c83c359122f973183688fca0ba2cf50e8", @"/Views/_ViewImports.cshtml")]
    public class Views_Management_ConfigAdmin : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 3 "C:\Users\golf_\Documents\GitHub\DailyLoan\DailyLoan\Views\Management\ConfigAdmin.cshtml"
   var init = new InitialConfig(); 

#line default
#line hidden
#nullable disable
            WriteLiteral(@"<div class=""row"">
    <div class=""col-sm-11 pt-2"">
        <div class=""row"">
            <h2>เรตราคา</h2>
        </div>
    </div>
    <div class=""col-sm-11 pt-2"">
        <div class=""row"">
            <div class=""col-md-4"">
                <div class=""form-group required-field"">
                    <label for=""acc-name"">");
#nullable restore
#line 14 "C:\Users\golf_\Documents\GitHub\DailyLoan\DailyLoan\Views\Management\ConfigAdmin.cshtml"
                                     Write(init.config_th["CustomerRate"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</label>\r\n                    <input type=\"text\" class=\"form-control\" name=\"customerrate\" id=\"customerrate\"");
            BeginWriteAttribute("value", " value=\"", 569, "\"", 618, 1);
#nullable restore
#line 15 "C:\Users\golf_\Documents\GitHub\DailyLoan\DailyLoan\Views\Management\ConfigAdmin.cshtml"
WriteAttributeValue("", 577, ViewBag.PageData.Configs["CustomerRate"], 577, 41, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" required>\r\n                </div><!-- End .form-group -->\r\n            </div><!-- End .col-md-8 -->\r\n            <div class=\"col-md-4\">\r\n                <div class=\"form-group required-field\">\r\n                    <label for=\"acc-name\">");
#nullable restore
#line 20 "C:\Users\golf_\Documents\GitHub\DailyLoan\DailyLoan\Views\Management\ConfigAdmin.cshtml"
                                     Write(init.config_th["AgentRate"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</label>\r\n                    <input type=\"text\" class=\"form-control\" name=\"agentrate\" id=\"agentrate\"");
            BeginWriteAttribute("value", " value=\"", 985, "\"", 1031, 1);
#nullable restore
#line 21 "C:\Users\golf_\Documents\GitHub\DailyLoan\DailyLoan\Views\Management\ConfigAdmin.cshtml"
WriteAttributeValue("", 993, ViewBag.PageData.Configs["AgentRate"], 993, 38, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" required>\r\n                </div><!-- End .form-group -->\r\n            </div><!-- End .col-md-8 -->\r\n            <div class=\"col-md-4\">\r\n                <div class=\"form-group required-field\">\r\n                    <label for=\"acc-name\">");
#nullable restore
#line 26 "C:\Users\golf_\Documents\GitHub\DailyLoan\DailyLoan\Views\Management\ConfigAdmin.cshtml"
                                     Write(init.config_th["HouseRate"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</label>\r\n                    <input type=\"text\" class=\"form-control\" name=\"houserate\" id=\"houserate\"");
            BeginWriteAttribute("value", " value=\"", 1398, "\"", 1444, 1);
#nullable restore
#line 27 "C:\Users\golf_\Documents\GitHub\DailyLoan\DailyLoan\Views\Management\ConfigAdmin.cshtml"
WriteAttributeValue("", 1406, ViewBag.PageData.Configs["HouseRate"], 1406, 38, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(@" required>
                </div><!-- End .form-group -->
            </div><!-- End .col-md-8 -->
        </div><!-- End .row -->
    </div><!-- End .col-sm-11 -->
    <div class=""col-sm-11 pt-2"">
        <div class=""row"">
            <h2>เงื่อนไขการตัด</h2>
        </div>
    </div>
    <div class=""col-sm-11 pt-2"">
        <div class=""row"">
            <div class=""col-md-6"">
                <div class=""form-group required-field"">
                    <label for=""acc-name"">");
#nullable restore
#line 41 "C:\Users\golf_\Documents\GitHub\DailyLoan\DailyLoan\Views\Management\ConfigAdmin.cshtml"
                                     Write(init.config_th["MinCutDay"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</label>\r\n                    <input type=\"text\" class=\"form-control\" name=\"mincutday\" id=\"mincutday\"");
            BeginWriteAttribute("value", " value=\"", 2066, "\"", 2112, 1);
#nullable restore
#line 42 "C:\Users\golf_\Documents\GitHub\DailyLoan\DailyLoan\Views\Management\ConfigAdmin.cshtml"
WriteAttributeValue("", 2074, ViewBag.PageData.Configs["MinCutDay"], 2074, 38, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" required>\r\n                </div><!-- End .form-group -->\r\n            </div><!-- End .col-md-8 -->\r\n            <div class=\"col-md-6\">\r\n                <div class=\"form-group required-field\">\r\n                    <label for=\"acc-name\">");
#nullable restore
#line 47 "C:\Users\golf_\Documents\GitHub\DailyLoan\DailyLoan\Views\Management\ConfigAdmin.cshtml"
                                     Write(init.config_th["IncCutCriteria"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</label>\r\n                    <input type=\"text\" class=\"form-control\" name=\"inccutcriteria\" id=\"inccutcriteria\"");
            BeginWriteAttribute("value", " value=\"", 2494, "\"", 2545, 1);
#nullable restore
#line 48 "C:\Users\golf_\Documents\GitHub\DailyLoan\DailyLoan\Views\Management\ConfigAdmin.cshtml"
WriteAttributeValue("", 2502, ViewBag.PageData.Configs["IncCutCriteria"], 2502, 43, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(@" required>
                </div><!-- End .form-group -->
            </div><!-- End .col-md-8 -->
        </div><!-- End .row -->
    </div><!-- End .col-sm-11 -->
    <div class=""col-sm-11 pt-2"">
        <div class=""row"">
            <div class=""col-md-6"">
                <div class=""form-group required-field"">
                    <label for=""acc-name"">");
#nullable restore
#line 57 "C:\Users\golf_\Documents\GitHub\DailyLoan\DailyLoan\Views\Management\ConfigAdmin.cshtml"
                                     Write(init.config_th["DecCutPercen"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</label>\r\n                    <input type=\"text\" class=\"form-control\" name=\"deccutpercen\" id=\"deccutpercen\"");
            BeginWriteAttribute("value", " value=\"", 3050, "\"", 3099, 1);
#nullable restore
#line 58 "C:\Users\golf_\Documents\GitHub\DailyLoan\DailyLoan\Views\Management\ConfigAdmin.cshtml"
WriteAttributeValue("", 3058, ViewBag.PageData.Configs["DecCutPercen"], 3058, 41, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" required>\r\n                </div><!-- End .form-group -->\r\n            </div><!-- End .col-md-8 -->\r\n            <div class=\"col-md-6\">\r\n                <div class=\"form-group required-field\">\r\n                    <label for=\"acc-name\">");
#nullable restore
#line 63 "C:\Users\golf_\Documents\GitHub\DailyLoan\DailyLoan\Views\Management\ConfigAdmin.cshtml"
                                     Write(init.config_th["DecCutCriteria"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</label>\r\n                    <input type=\"text\" class=\"form-control\" name=\"deccutcriteria\" id=\"deccutcriteria\"");
            BeginWriteAttribute("value", " value=\"", 3481, "\"", 3532, 1);
#nullable restore
#line 64 "C:\Users\golf_\Documents\GitHub\DailyLoan\DailyLoan\Views\Management\ConfigAdmin.cshtml"
WriteAttributeValue("", 3489, ViewBag.PageData.Configs["DecCutCriteria"], 3489, 43, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(@" required>
                </div><!-- End .form-group -->
            </div><!-- End .col-md-8 -->
        </div><!-- End .row -->
    </div><!-- End .col-sm-11 -->
    <div class=""col-sm-11 pt-2"">
        <div class=""row"">
            <h2>เรตกำไร และ เงื่อนไขพิเศษ</h2>
        </div>
    </div>
    <div class=""col-sm-11 pt-2"">
        <div class=""row"">
            <div class=""col-md-6"">
                <div class=""form-group required-field"">
                    <label for=""acc-name"">");
#nullable restore
#line 78 "C:\Users\golf_\Documents\GitHub\DailyLoan\DailyLoan\Views\Management\ConfigAdmin.cshtml"
                                     Write(init.config_th["TotalProfit"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</label>\r\n                    <input type=\"text\" class=\"form-control\" name=\"totalprofit\" id=\"totalprofit\"");
            BeginWriteAttribute("value", " value=\"", 4171, "\"", 4219, 1);
#nullable restore
#line 79 "C:\Users\golf_\Documents\GitHub\DailyLoan\DailyLoan\Views\Management\ConfigAdmin.cshtml"
WriteAttributeValue("", 4179, ViewBag.PageData.Configs["TotalProfit"], 4179, 40, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" required>\r\n                </div><!-- End .form-group -->\r\n            </div><!-- End .col-md-8 -->\r\n            <div class=\"col-md-6\">\r\n                <div class=\"form-group required-field\">\r\n                    <label for=\"acc-name\">");
#nullable restore
#line 84 "C:\Users\golf_\Documents\GitHub\DailyLoan\DailyLoan\Views\Management\ConfigAdmin.cshtml"
                                     Write(init.config_th["SpecialRateCriteria"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</label>\r\n                    <input type=\"text\" class=\"form-control\" name=\"specialratecriteria\" id=\"specialratecriteria\"");
            BeginWriteAttribute("value", " value=\"", 4616, "\"", 4672, 1);
#nullable restore
#line 85 "C:\Users\golf_\Documents\GitHub\DailyLoan\DailyLoan\Views\Management\ConfigAdmin.cshtml"
WriteAttributeValue("", 4624, ViewBag.PageData.Configs["SpecialRateCriteria"], 4624, 48, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(@" required>
                </div><!-- End .form-group -->
            </div><!-- End .col-md-8 -->
        </div><!-- End .row -->
    </div><!-- End .col-sm-11 -->
    <div class=""col-sm-11 pt-2"">
        <div class=""row"">
            <h2>การแจ้งเตือน</h2>
        </div>
    </div>
    <div class=""col-sm-11 pt-2"">
        <div class=""row"">
            <div class=""col-md-6"">
                <div class=""form-group required-field"">
                    <label for=""acc-name"">");
#nullable restore
#line 99 "C:\Users\golf_\Documents\GitHub\DailyLoan\DailyLoan\Views\Management\ConfigAdmin.cshtml"
                                     Write(init.config_th["NotPayAlert"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</label>\r\n                    <input type=\"text\" class=\"form-control\" name=\"notpayalert\" id=\"notpayalert\"");
            BeginWriteAttribute("value", " value=\"", 5298, "\"", 5346, 1);
#nullable restore
#line 100 "C:\Users\golf_\Documents\GitHub\DailyLoan\DailyLoan\Views\Management\ConfigAdmin.cshtml"
WriteAttributeValue("", 5306, ViewBag.PageData.Configs["NotPayAlert"], 5306, 40, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" required>\r\n                </div><!-- End .form-group -->\r\n            </div><!-- End .col-md-8 -->\r\n            <div class=\"col-md-6\">\r\n                <div class=\"form-group required-field\">\r\n                    <label for=\"acc-name\">");
#nullable restore
#line 105 "C:\Users\golf_\Documents\GitHub\DailyLoan\DailyLoan\Views\Management\ConfigAdmin.cshtml"
                                     Write(init.config_th["PartialPayAlert"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</label>\r\n                    <input type=\"text\" class=\"form-control\" name=\"partialpayalert\" id=\"partialpayalert\"");
            BeginWriteAttribute("value", " value=\"", 5731, "\"", 5783, 1);
#nullable restore
#line 106 "C:\Users\golf_\Documents\GitHub\DailyLoan\DailyLoan\Views\Management\ConfigAdmin.cshtml"
WriteAttributeValue("", 5739, ViewBag.PageData.Configs["PartialPayAlert"], 5739, 44, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" required>\r\n                </div><!-- End .form-group -->\r\n            </div><!-- End .col-md-8 -->\r\n        </div><!-- End .row -->\r\n    </div><!-- End .col-sm-11 -->\r\n</div>\r\n<div class=\"mb-1\"></div>\r\n<input type=\"hidden\" id=\"HouseId\" name=\"HouseId\"");
            BeginWriteAttribute("value", " value=\"", 6036, "\"", 6058, 1);
#nullable restore
#line 113 "C:\Users\golf_\Documents\GitHub\DailyLoan\DailyLoan\Views\Management\ConfigAdmin.cshtml"
WriteAttributeValue("", 6044, ViewBag.House, 6044, 14, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(@" />
<div class=""required text-right"">* Required Field</div>
<label id=""msg""></label>
<div class=""form-footer"">
    <div class=""form-footer-right"">
        <button type=""button"" class=""btn btn-primary"" id=""save"">บันทึก</button>
    </div>
</div><!-- End .form-footer -->
<div class=""row"">
    <div class=""col-12"">
        <table id=""table"" class=""table table-striped table-bordered"" style=""width:100%"">
            <thead style=""width:100%"">
                <tr>
                    <th>#</th>
                    <th>วันที่เริ่ม</th>
                    <th>วันที่หมด</th>
                    <th>เรตลูกค้า</th>
                    <th>เรตค่าหัว</th>
                    <th>เรตเข้าบ้าน</th>
                    <th>จำนวนวันตัด</th>
                    <th></th>
                </tr>
            </thead>
            <tbody style=""width:100%"">
");
#nullable restore
#line 137 "C:\Users\golf_\Documents\GitHub\DailyLoan\DailyLoan\Views\Management\ConfigAdmin.cshtml"
                 if (ViewBag.PageData.SpecialRates.Count > 0)
                {
                    foreach (var items in ViewBag.PageData.SpecialRates)
                    {

#line default
#line hidden
#nullable disable
            WriteLiteral("                        <tr class=\'clickable-row\'>\r\n                            <td scope=\"row\">");
#nullable restore
#line 142 "C:\Users\golf_\Documents\GitHub\DailyLoan\DailyLoan\Views\Management\ConfigAdmin.cshtml"
                                       Write(items.Id);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                            <td>");
#nullable restore
#line 143 "C:\Users\golf_\Documents\GitHub\DailyLoan\DailyLoan\Views\Management\ConfigAdmin.cshtml"
                           Write(items.StartDate.ToString("dd/MM/yyyy"));

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                            <td>");
#nullable restore
#line 144 "C:\Users\golf_\Documents\GitHub\DailyLoan\DailyLoan\Views\Management\ConfigAdmin.cshtml"
                           Write(items.EndDate.ToString("dd/MM/yyyy"));

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                            <td>");
#nullable restore
#line 145 "C:\Users\golf_\Documents\GitHub\DailyLoan\DailyLoan\Views\Management\ConfigAdmin.cshtml"
                           Write(items.CustomerRate);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                            <td>");
#nullable restore
#line 146 "C:\Users\golf_\Documents\GitHub\DailyLoan\DailyLoan\Views\Management\ConfigAdmin.cshtml"
                           Write(items.AgentRate);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                            <td>");
#nullable restore
#line 147 "C:\Users\golf_\Documents\GitHub\DailyLoan\DailyLoan\Views\Management\ConfigAdmin.cshtml"
                           Write(items.HouseRate);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                            <td>");
#nullable restore
#line 148 "C:\Users\golf_\Documents\GitHub\DailyLoan\DailyLoan\Views\Management\ConfigAdmin.cshtml"
                           Write(items.MinCutDay);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</td>
                            <td>
                                <button type=""button"" class=""btn btnClick"" style=""padding-left:10px;padding-right:10px;padding-top:5px;padding-bottom:5px;"">select</button>
                            </td>
                        </tr>
");
#nullable restore
#line 153 "C:\Users\golf_\Documents\GitHub\DailyLoan\DailyLoan\Views\Management\ConfigAdmin.cshtml"
                    }
                }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"            </tbody>
        </table>
    </div>
</div>
<div class=""row"">
    <div class=""col-sm-11 pt-2"">
        <input type=""checkbox"" id=""isNew"" name=""isNew"" value=""true"">
        <label for=""isNew"">เพิ่มเรตพิเศษใหม่</label>
    </div>
    <div class=""col-sm-11 pt-2"">
        <div class=""row"">
            <div class=""col-md-6"">
                <div class=""form-group required-field"">
                    <label for=""acc-name"">วันที่เริ่ม</label>
                    <input type=""text"" class=""form-control"" name=""startdate"" id=""startdate"" required>
                </div><!-- End .form-group -->
            </div><!-- End .col-md-8 -->
            <div class=""col-md-6"">
                <div class=""form-group required-field"">
                    <label for=""acc-name"">วันที่หมด</label>
                    <input type=""text"" class=""form-control"" name=""enddate"" id=""enddate"" required>
                </div><!-- End .form-group -->
            </div><!-- End .col-md-8 -->
        </div><!-- End");
            WriteLiteral(" .row -->\r\n    </div><!-- End .col-sm-11 -->\r\n    <div class=\"col-sm-11 pt-2\">\r\n        <div class=\"row\">\r\n            <div class=\"col-md-4\">\r\n                <div class=\"form-group required-field\">\r\n                    <label for=\"acc-name\">");
#nullable restore
#line 184 "C:\Users\golf_\Documents\GitHub\DailyLoan\DailyLoan\Views\Management\ConfigAdmin.cshtml"
                                     Write(init.config_th["CustomerRate"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</label>
                    <input type=""text"" class=""form-control"" name=""spcustomerrate"" id=""spcustomerrate"" required>
                </div><!-- End .form-group -->
            </div><!-- End .col-md-8 -->
            <div class=""col-md-4"">
                <div class=""form-group required-field"">
                    <label for=""acc-name"">");
#nullable restore
#line 190 "C:\Users\golf_\Documents\GitHub\DailyLoan\DailyLoan\Views\Management\ConfigAdmin.cshtml"
                                     Write(init.config_th["AgentRate"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</label>
                    <input type=""text"" class=""form-control"" name=""spagentrate"" id=""spagentrate"" required>
                </div><!-- End .form-group -->
            </div><!-- End .col-md-8 -->
            <div class=""col-md-4"">
                <div class=""form-group required-field"">
                    <label for=""acc-name"">");
#nullable restore
#line 196 "C:\Users\golf_\Documents\GitHub\DailyLoan\DailyLoan\Views\Management\ConfigAdmin.cshtml"
                                     Write(init.config_th["HouseRate"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</label>
                    <input type=""text"" class=""form-control"" name=""sphouserate"" id=""sphouserate"" required>
                </div><!-- End .form-group -->
            </div><!-- End .col-md-8 -->
        </div><!-- End .row -->
    </div><!-- End .col-sm-11 -->
    <div class=""col-sm-11 pt-2"">
        <div class=""row"">
            <div class=""col-md-8"">
                <div class=""form-group required-field"">
                    <label for=""acc-name"">");
#nullable restore
#line 206 "C:\Users\golf_\Documents\GitHub\DailyLoan\DailyLoan\Views\Management\ConfigAdmin.cshtml"
                                     Write(init.config_th["MinCutDay"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</label>
                    <input type=""text"" class=""form-control"" name=""spmincutday"" id=""spmincutday"" required>
                </div><!-- End .form-group -->
            </div><!-- End .col-md-8 -->
        </div><!-- End .row -->
    </div><!-- End .col-sm-11 -->
</div>
<div class=""mb-1""></div>
<input type=""hidden"" id=""SpecialRateId"" name=""SpecialRateId"" />
<div class=""required text-right"">* Required Field</div>
<label id=""msg""></label>
<div class=""form-footer"">
    <div class=""form-footer-right"">
        <button type=""button"" class=""btn btn-danger"" id=""spdelete"" style=""display:none;"">ลบ</button>
        <button type=""button"" class=""btn btn-primary"" id=""spsave"">บันทึก</button>
    </div>
</div><!-- End .form-footer -->
<script type=""text/javascript"">
    var root = '");
#nullable restore
#line 224 "C:\Users\golf_\Documents\GitHub\DailyLoan\DailyLoan\Views\Management\ConfigAdmin.cshtml"
           Write(Url.Content("~/"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"';
    root = (root == '/' ? """" : root);
    function dateStr(dat) {
        try {
            var arr = dat.split('T');
            var arr = arr[0].split('-');
            if (arr.length == 3)
                return arr[2] + '/' + arr[1] + '/' + arr[0];
            else return null;

        } catch (e) {
            console.log(e);
            return null;
        }
    }
$(document).ready(function () {
    $('#isNew').prop('checked', true);
    $(""#SpecialRateId"").val("""");
    $('#isNew').click(function () {
        if ($('#isNew').prop('checked')) {
            $(""#SpecialRateId"").val("""");
            $('#spdelete').hide();
        } else {
            if ($(""#SpecialRateId"").val() != """") $('#spdelete').show();
        }
    });
    $('#table tbody').on('click', '.btnClick', function () {
        $('#spdelete').show();
        $(""#SpecialRateId"").val($(this).closest('tr').find('td').eq(0).text());
        $.ajax({
            type: ""GET"",
            url: root + ""/GetSpecia");
            WriteLiteral(@"lRateDetail/"" + $(""#SpecialRateId"").val(),
            contentType: ""application/json; charset=utf-8;"",
            dataType: ""json"",
            success: function (data) {
                if (data != undefined) {
                    $(""#isNew"").prop('checked', false);
                    $(""#startdate"").val(dateStr(data.startDate));
                    $(""#enddate"").val(dateStr(data.endDate));
                    $(""#spcustomerrate"").val(data.customerRate);
                    $(""#spagentrate"").val(data.agentRate);
                    $(""#sphouserate"").val(data.houseRate);
                    $(""#spmincutday"").val(data.minCutDay);
                }
            },
            error: function (e) { $(""#HouseId"").val(""""); console.log(e) }
        });
    });
    $(""#save"").click(function () {
        if ($(""#HouseId"").val() != """") {
            let req = new XMLHttpRequest();
            let formData = new FormData();
            
            formData.append(""HouseId"", $(""#HouseId"").val())");
            WriteLiteral(@";
            formData.append(""CustomerRate"", $(""#customerrate"").val());
            formData.append(""AgentRate"", $(""#agentrate"").val());
            formData.append(""HouseRate"", $(""#houserate"").val());
            formData.append(""MinCutDay"", $(""#mincutday"").val());
            formData.append(""IncCutCriteria"", $(""#inccutcriteria"").val());
            formData.append(""DecCutCriteria"", $(""#deccutcriteria"").val());
            formData.append(""DecCutPercen"", $(""#deccutpercen"").val());
            formData.append(""SpecialRateCriteria"", $(""#specialratecriteria"").val());
            formData.append(""TotalProfit"", $(""#totalprofit"").val());
            formData.append(""NotPayAlert"", $(""#notpayalert"").val());
            formData.append(""PartialPayAlert"", $(""#partialpayalert"").val());
            req.open(""POST"", '/EditConfig');
            req.send(formData);
            req.onload = function () {
                if (req.readyState != 4 || req.status != 200) {
                    $(""#msg"").text(req.");
            WriteLiteral(@"responseText)
                } else {
                    alert(req.responseText);
                    location.reload();
                }
            }
        } else {
            alert('กรุณาเลือกบ้าน');
        }
    });
    $(""#spsave"").click(function () {
        if ($(""#SpecialRateId"").val() != """" && !$(""#isNew"").prop('checked') || ($(""#isNew"").prop('checked') && $(""#SpecialRateId"").val() == '')) {
            let req = new XMLHttpRequest();
            let formData = new FormData();

            formData.append(""isNew"", $(""#isNew"").prop('checked'));
            formData.append(""Id"", $(""#SpecialRateId"").val());
            formData.append(""HouseId"", $(""#HouseId"").val());
            formData.append(""StartDate"", $(""#startdate"").val());
            formData.append(""EndDate"", $(""#enddate"").val());
            formData.append(""CustomerRate"", $(""#spcustomerrate"").val());
            formData.append(""AgentRate"", $(""#spagentrate"").val());
            formData.append(""HouseRate"", $(""#sp");
            WriteLiteral(@"houserate"").val());
            formData.append(""MinCutDay"", $(""#spmincutday"").val());
            req.open(""POST"", '/EditSpecialRate');
            req.send(formData);
            req.onload = function () {
                if (req.readyState != 4 || req.status != 200) {
                    $(""#msg"").text(req.responseText)
                } else {
                    alert(req.responseText);
                    location.reload();
                }
            }
        } else {
            alert('กรุณาเลือกเรตพิเศษ หรือ ติ๊กที่เพิ่มเรตพิเศษใหม่');
        }
    });
    $(""#spdelete"").click(function () {
        if (confirm('คุณต้องการที่จะลบเรตพิเศษนี้ ใช่หรือไม่?')) {
            let req = new XMLHttpRequest();
            req.open(""GET"", '/DeleteSpecialRate/' + $(""#SpecialRateId"").val());
            req.send();
            req.onload = function () {
                if (req.readyState != 4 || req.status != 200) {
                    $(""#msg"").text(req.responseText)
                } ");
            WriteLiteral("else {\r\n                    alert(req.responseText);\r\n                    location.reload();\r\n                }\r\n            }\r\n        }\r\n    });\r\n});\r\n</script>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
