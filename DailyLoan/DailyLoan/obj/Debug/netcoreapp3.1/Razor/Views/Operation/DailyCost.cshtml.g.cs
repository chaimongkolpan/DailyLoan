#pragma checksum "C:\Project\DailyLoan\DailyLoan\DailyLoan\DailyLoan\Views\Operation\DailyCost.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "ebb33250d9ce40cb6499919094ddebca23f7b3ae"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Operation_DailyCost), @"mvc.1.0.view", @"/Views/Operation/DailyCost.cshtml")]
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
#line 1 "C:\Project\DailyLoan\DailyLoan\DailyLoan\DailyLoan\Views\_ViewImports.cshtml"
using DailyLoan;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Project\DailyLoan\DailyLoan\DailyLoan\DailyLoan\Views\_ViewImports.cshtml"
using DailyLoan.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ebb33250d9ce40cb6499919094ddebca23f7b3ae", @"/Views/Operation/DailyCost.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ce9f4d3c83c359122f973183688fca0ba2cf50e8", @"/Views/_ViewImports.cshtml")]
    public class Views_Operation_DailyCost : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("<div class=\"container\">\r\n    <div class=\"col-md-8 offset-md-2\">\r\n        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "ebb33250d9ce40cb6499919094ddebca23f7b3ae3322", async() => {
                WriteLiteral(@"
            <div class=""jumbotron"">
                <div class=""row alert alert-warning"" role=""alert"">
                    <div class=""col align-self-start"">สรุปค่าใช้จ่ายรายวัน</div>
                    <div class=""col align-self-end"">
                        <div class=""col-4 input-group date"" data-provide=""datepicker"">
                            <input type=""text"" class=""form-control"" data-date-format=""dd/mm/yyyy"" id=""datepicker"">
                            <div class=""input-group-addon"">
                                <span class=""glyphicon glyphicon-th""></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class=""card mt-1"">
                    <div class=""card-header"">
                        รายจ่าย
                    </div>
                    <div class=""card-body"">
                        <div class=""row col-12 "">
                            <label class=""col-sm-2 col-form-label"">น้ำมัน");
                WriteLiteral(@"</label>
                            <div class=""col-sm-4"">
                                <input type=""text"" id=""oil"">
                            </div>
                            <label class=""col-sm-2 col-form-label"">ค่าซ่อมรถ</label>
                            <div class=""col-sm-4"">
                                <input type=""text"" id=""maintenance"">
                            </div>
                        </div>
                        <div class=""row col-12 "">
                            <label class=""col-sm-2 col-form-label"">เบี้ยเลี้ยงเติมเงิน</label>
                            <div class=""col-sm-4"">
                                <input type=""text"" id=""card"">
                            </div>
                            <label class=""col-sm-2 col-form-label"">ค่าหัว</label>
                            <div class=""col-sm-4"">
                                <input type=""text"" id=""head"">
                            </div>
                        </div>
                        ");
                WriteLiteral(@"<div class=""row col-12 "">
                            <label class=""col-sm-2 col-form-label"">ต่าใช้จ่ายอื่นๆ</label>
                            <div class=""col-sm-4"">
                                <input type=""text"" id=""other"">
                            </div>
                        </div>
                    </div>
                </div>
                <div class=""card mt-1"">
                    <div class=""card-header"">
                        รายรับ
                    </div>
                    <div class=""card-body"">
                        <div class=""row col-12 "">
                            <label class=""col-sm-2 col-form-label"">ยอดที่เก็บได้</label>
                            <div class=""col-sm-4"">
                                <input type=""text"" id=""income"">
                            </div>
                        </div>
                    </div>
                </div>
                <div class=""card mt-1"">
                    <div class=""card-header"">
          ");
                WriteLiteral(@"              สรุป
                    </div>
                    <div class=""card-body"">
                        <div class=""row col-12 "">
                            <label class=""col-sm-2 col-form-label"">ยอดที่ต้องเก็บ</label>
                            <div class=""col-sm-4"">
                                <input type=""text"" id=""income"">
                            </div>
                        </div>
                        <div class=""row col-12 "">
                            <label class=""col-sm-2 col-form-label"">เงินคงเหลือ</label>
                            <div class=""col-sm-4"">
                                <input type=""text"" id=""income"">
                            </div>
                        </div>
                    </div>
                </div>
                <div class=""row mt-2"">
                    <div class=""col text-center"">
                        <button type=""submit"" class=""btn btn-primary"">Save</button>
                    </div>
                </div>
");
                WriteLiteral("            </div>\r\n        ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(@"
    </div>
</div>
<script src=""https://code.jquery.com/jquery-3.3.1.min.js""></script>
<script src=""https://unpkg.com/gijgo@1.9.13/js/gijgo.min.js"" type=""text/javascript""></script>
<link href=""https://unpkg.com/gijgo@1.9.13/css/gijgo.min.css"" rel=""stylesheet"" type=""text/css"" />
<script type=""text/javascript"">
    var root = '");
#nullable restore
#line 94 "C:\Project\DailyLoan\DailyLoan\DailyLoan\DailyLoan\Views\Operation\DailyCost.cshtml"
           Write(Url.Content("~/"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\';\r\n    root = (root == \'/\' ? \"\" : root);\r\n    $(document).ready(function () {\r\n        $(\'#datepicker\').datepicker({\r\n            format: \'dd/mm/yyyy\'\r\n        });\r\n    });\r\n</script>");
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
