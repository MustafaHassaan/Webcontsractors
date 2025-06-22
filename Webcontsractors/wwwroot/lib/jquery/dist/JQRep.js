$(document).ready(function () {
    //PSR Report
    $("#Pdfpsr").click(function () {

        var printContent = document.getElementById("PSR-to-pdf").innerHTML;
        var originalContents = document.body.innerHTML;
        // إخفاء الهيدر والفوتر
        $('header, footer').hide();
        // افتح نافذة جديدة للطباعة
        document.body.innerHTML = printContent; // ضع الجزء الذي تود طباعته فقط
        window.print(); // اطبع المحتوى
        document.body.innerHTML = originalContents; // رجع الصفحة زي ما كانت
        location.reload();
    });
    $("#Excelpsr").click(function () {
        window.print = null;
        var projectReportModel = [];
        $('#PSRep tbody tr').each(function () {
            var row = {
                ProjectName: $(this).find('td:eq(0)').text().trim(),
                LastTransactionDate: $(this).find('td:eq(1)').text().trim(),
                CreditorSum: parseFloat($(this).find('td:eq(2)').text().trim()) || 0,
                DebitorSum: parseFloat($(this).find('td:eq(3)').text().trim()) || 0,
                Balance: parseFloat($(this).find('td:eq(4)').text().trim()) || 0,
                PartnerName: $(this).find('td:eq(5)').text().trim(),
                Note: $(this).find('td:eq(6)').text().trim()
            };
            projectReportModel.push(row);
        });
        $.ajax({
            url: '/PSReport/ExportToExcel', // تأكد من أن هذا هو المسار الصحيح للـ action
            type: 'POST',
            contentType: "application/json; charset=utf-8", // تأكد من أن content-type مضبوط
            data: JSON.stringify(projectReportModel), // أرسل البيانات المحوّلة إلى JSON
            xhrFields: {
                responseType: 'blob' // مهم لاستقبال البيانات كـ Blob
            },
            success: function (blob, status, xhr) {
                // إنشاء رابط تنزيل مؤقت
                const url = window.URL.createObjectURL(blob);
                const a = document.createElement('a');
                a.href = url;

                // الحصول على اسم الملف من ترويسة Content-Disposition (إذا كانت موجودة)
                const filename = xhr.getResponseHeader('Content-Disposition')?.split('filename=')[1]?.split(';')[0]?.replace(/"/g, '');
                a.download = 'تقرير مختصر المشروعات.xlsx'; // استخدم الاسم من الخادم أو اسم افتراضي

                document.body.appendChild(a);
                a.click();

                // تنظيف الـ URL المؤقت
                window.URL.revokeObjectURL(url);
                document.body.removeChild(a); // إزالة الرابط من DOM
                location.reload();
            },
            error: function (xhr, status, error) {
                console.error('خطأ في تصدير البيانات:', error);
                // يمكنك هنا عرض رسالة خطأ للمستخدم
            }
        });
    });
    //========================

    //PDR Report
    $("#Pdfpdr").click(function () {

        var printContent = document.getElementById("PDR-to-pdf").innerHTML;
        var originalContents = document.body.innerHTML;
        // إخفاء الهيدر والفوتر
        $('header, footer').hide();
        // افتح نافذة جديدة للطباعة
        document.body.innerHTML = printContent; // ضع الجزء الذي تود طباعته فقط
        window.print(); // اطبع المحتوى
        document.body.innerHTML = originalContents; // رجع الصفحة زي ما كانت
        location.reload();
    });
    $("#Excelpdr").click(function () {
        window.print = null;
        var projectReportModel = [];
        $('#PDRep tbody tr').each(function () {
            var row = {
                Partnername: $(this).find('td:eq(0)').text().trim(),
                Projectname: $(this).find('td:eq(1)').text().trim(),
                Detailes: $(this).find('td:eq(2)').text().trim(),
                Tdate: $(this).find('td:eq(3)').text().trim(),
                Creditor: parseFloat($(this).find('td:eq(4)').text().trim()) || 0,
                Debitor: parseFloat($(this).find('td:eq(5)').text().trim()) || 0,
                Vatamount: parseFloat($(this).find('td:eq(6)').text().trim()) || 0,
                Balance: parseFloat($(this).find('td:eq(7)').text().trim()) || 0,
                Note: $(this).find('td:eq(8)').text().trim(),
            };
            projectReportModel.push(row);
        });
        $.ajax({
            url: '/PDReport/ExportToExcel', // تأكد من أن هذا هو المسار الصحيح للـ action
            type: 'POST',
            contentType: "application/json; charset=utf-8", // تأكد من أن content-type مضبوط
            data: JSON.stringify(projectReportModel), // أرسل البيانات المحوّلة إلى JSON
            xhrFields: {
                responseType: 'blob' // مهم لاستقبال البيانات كـ Blob
            },
            success: function (blob, status, xhr) {
                // إنشاء رابط تنزيل مؤقت
                const url = window.URL.createObjectURL(blob);
                const a = document.createElement('a');
                a.href = url;

                // الحصول على اسم الملف من ترويسة Content-Disposition (إذا كانت موجودة)
                const filename = xhr.getResponseHeader('Content-Disposition')?.split('filename=')[1]?.split(';')[0]?.replace(/"/g, '');
                a.download = 'تقرير تفصيلي المشروعات.xlsx'; // استخدم الاسم من الخادم أو اسم افتراضي

                document.body.appendChild(a);
                a.click();

                // تنظيف الـ URL المؤقت
                window.URL.revokeObjectURL(url);
                document.body.removeChild(a); // إزالة الرابط من DOM
                location.reload();
            },
            error: function (xhr, status, error) {
                console.error('خطأ في تصدير البيانات:', error);
                // يمكنك هنا عرض رسالة خطأ للمستخدم
            }
        });
    });
    //========================

    //Sales Report
    $("#Pdfsales").click(function () {

        var printContent = document.getElementById("Sales-to-pdf").innerHTML;
        var originalContents = document.body.innerHTML;
        // إخفاء الهيدر والفوتر
        $('header, footer').hide();
        // افتح نافذة جديدة للطباعة
        document.body.innerHTML = printContent; // ضع الجزء الذي تود طباعته فقط
        window.print(); // اطبع المحتوى
        document.body.innerHTML = originalContents; // رجع الصفحة زي ما كانت
        location.reload();
    });
    $("#Excelsales").click(function () {
        window.print = null;
        var projectReportModel = [];
        $('#SalRep tbody tr').each(function () {
            var row = {
                Projectname: $(this).find('td:eq(0)').text().trim(),
                Detailes: $(this).find('td:eq(1)').text().trim(),
                Creditor: parseFloat($(this).find('td:eq(2)').text().trim()) || 0,
                Vatamount: parseFloat($(this).find('td:eq(3)').text().trim()) || 0,
                Tdate: $(this).find('td:eq(4)').text().trim(),
                Note: $(this).find('td:eq(5)').text().trim(),
            };
            projectReportModel.push(row);
        });
        $.ajax({
            url: '/SalesReport/ExportToExcel', // تأكد من أن هذا هو المسار الصحيح للـ action
            type: 'POST',
            contentType: "application/json; charset=utf-8", // تأكد من أن content-type مضبوط
            data: JSON.stringify(projectReportModel), // أرسل البيانات المحوّلة إلى JSON
            xhrFields: {
                responseType: 'blob' // مهم لاستقبال البيانات كـ Blob
            },
            success: function (blob, status, xhr) {
                // إنشاء رابط تنزيل مؤقت
                const url = window.URL.createObjectURL(blob);
                const a = document.createElement('a');
                a.href = url;

                // الحصول على اسم الملف من ترويسة Content-Disposition (إذا كانت موجودة)
                const filename = xhr.getResponseHeader('Content-Disposition')?.split('filename=')[1]?.split(';')[0]?.replace(/"/g, '');
                a.download = 'تقرير المبيعات.xlsx'; // استخدم الاسم من الخادم أو اسم افتراضي

                document.body.appendChild(a);
                a.click();

                // تنظيف الـ URL المؤقت
                window.URL.revokeObjectURL(url);
                document.body.removeChild(a); // إزالة الرابط من DOM
                location.reload();
            },
            error: function (xhr, status, error) {
                console.error('خطأ في تصدير البيانات:', error);
                // يمكنك هنا عرض رسالة خطأ للمستخدم
            }
        });
    });
    //========================

    //Purchaces Report
    $("#Pdpur").click(function () {

        var printContent = document.getElementById("Sales-to-pdf").innerHTML;
        var originalContents = document.body.innerHTML;
        // إخفاء الهيدر والفوتر
        $('header, footer').hide();
        // افتح نافذة جديدة للطباعة
        document.body.innerHTML = printContent; // ضع الجزء الذي تود طباعته فقط
        window.print(); // اطبع المحتوى
        document.body.innerHTML = originalContents; // رجع الصفحة زي ما كانت
        location.reload();
    });
    $("#Excelpur").click(function () {
        window.print = null;
        var projectReportModel = [];
        $('#PurRep tbody tr').each(function () {
            var row = {
                Projectname: $(this).find('td:eq(0)').text().trim(),
                Detailes: $(this).find('td:eq(1)').text().trim(),
                Debitor: parseFloat($(this).find('td:eq(2)').text().trim()) || 0,
                Vatamount: parseFloat($(this).find('td:eq(3)').text().trim()) || 0,
                Tdate: $(this).find('td:eq(4)').text().trim(),
                Note: $(this).find('td:eq(5)').text().trim(),
            };
            projectReportModel.push(row);
        });
        $.ajax({
            url: '/PurchaseReport/ExportToExcel', // تأكد من أن هذا هو المسار الصحيح للـ action
            type: 'POST',
            contentType: "application/json; charset=utf-8", // تأكد من أن content-type مضبوط
            data: JSON.stringify(projectReportModel), // أرسل البيانات المحوّلة إلى JSON
            xhrFields: {
                responseType: 'blob' // مهم لاستقبال البيانات كـ Blob
            },
            success: function (blob, status, xhr) {
                // إنشاء رابط تنزيل مؤقت
                const url = window.URL.createObjectURL(blob);
                const a = document.createElement('a');
                a.href = url;

                // الحصول على اسم الملف من ترويسة Content-Disposition (إذا كانت موجودة)
                const filename = xhr.getResponseHeader('Content-Disposition')?.split('filename=')[1]?.split(';')[0]?.replace(/"/g, '');
                a.download = 'تقرير المشتريات.xlsx'; // استخدم الاسم من الخادم أو اسم افتراضي

                document.body.appendChild(a);
                a.click();

                // تنظيف الـ URL المؤقت
                window.URL.revokeObjectURL(url);
                document.body.removeChild(a); // إزالة الرابط من DOM
                location.reload();
            },
            error: function (xhr, status, error) {
                console.error('خطأ في تصدير البيانات:', error);
                // يمكنك هنا عرض رسالة خطأ للمستخدم
            }
        });
    });
    //========================


    //PT Report
    $("#Pdfptr").click(function () {

        var printContent = document.getElementById("PTR-to-pdf").innerHTML;
        var originalContents = document.body.innerHTML;
        // إخفاء الهيدر والفوتر
        $('header, footer').hide();
        // افتح نافذة جديدة للطباعة
        document.body.innerHTML = printContent; // ضع الجزء الذي تود طباعته فقط
        window.print(); // اطبع المحتوى
        document.body.innerHTML = originalContents; // رجع الصفحة زي ما كانت
        location.reload();
    });
    //$("#Excelptr").click(function () {
    //    window.print = null;
    //    var projectReportModel = [];
    //    $('#ptrRep tbody tr').each(function () {
    //        var row = {
    //            Tdate: $(this).find('td:eq(4)').text().trim(),
    //            Projectname: $(this).find('td:eq(0)').text().trim(),
    //            Detailes: $(this).find('td:eq(1)').text().trim(),
    //            Debitor: parseFloat($(this).find('td:eq(2)').text().trim()) || 0,
    //            Vatamount: parseFloat($(this).find('td:eq(3)').text().trim()) || 0,
    //        };
    //        projectReportModel.push(row);
    //    });
    //    $.ajax({
    //        url: '/PTRReport/ExportToExcel', // تأكد من أن هذا هو المسار الصحيح للـ action
    //        type: 'POST',
    //        contentType: "application/json; charset=utf-8", // تأكد من أن content-type مضبوط
    //        data: JSON.stringify(projectReportModel), // أرسل البيانات المحوّلة إلى JSON
    //        xhrFields: {
    //            responseType: 'blob' // مهم لاستقبال البيانات كـ Blob
    //        },
    //        success: function (blob, status, xhr) {
    //            // إنشاء رابط تنزيل مؤقت
    //            const url = window.URL.createObjectURL(blob);
    //            const a = document.createElement('a');
    //            a.href = url;

    //            // الحصول على اسم الملف من ترويسة Content-Disposition (إذا كانت موجودة)
    //            const filename = xhr.getResponseHeader('Content-Disposition')?.split('filename=')[1]?.split(';')[0]?.replace(/"/g, '');
    //            a.download = 'تقرير حركة مشروع.xlsx'; // استخدم الاسم من الخادم أو اسم افتراضي

    //            document.body.appendChild(a);
    //            a.click();

    //            // تنظيف الـ URL المؤقت
    //            window.URL.revokeObjectURL(url);
    //            document.body.removeChild(a); // إزالة الرابط من DOM
    //            location.reload();
    //        },
    //        error: function (xhr, status, error) {
    //            console.error('خطأ في تصدير البيانات:', error);
    //            // يمكنك هنا عرض رسالة خطأ للمستخدم
    //        }
    //    });
    //});
    //========================


    //PT Report
    $("#Pdfdtr").click(function () {

        var printContent = document.getElementById("DTR-to-pdf").innerHTML;
        var originalContents = document.body.innerHTML;
        // إخفاء الهيدر والفوتر
        $('header, footer').hide();
        // افتح نافذة جديدة للطباعة
        document.body.innerHTML = printContent; // ضع الجزء الذي تود طباعته فقط
        window.print(); // اطبع المحتوى
        document.body.innerHTML = originalContents; // رجع الصفحة زي ما كانت
        location.reload();
    });
    $("#Exceldtr").click(function () {
        window.print = null;
        var projectReportModel = [];
        $('#DTRep tbody tr').each(function () {
            var row = {
                Partnername: $(this).find('td:eq(0)').text().trim(),
                Projectname: $(this).find('td:eq(1)').text().trim(),
                Tdate: $(this).find('td:eq(2)').text().trim(),
                Debitor: parseFloat($(this).find('td:eq(3)').text().trim()) || 0,
                Vatamount: parseFloat($(this).find('td:eq(4)').text().trim()) || 0,
                Note: $(this).find('td:eq(5)').text().trim(),
            };
            projectReportModel.push(row);
        });
        $.ajax({
            url: '/DTReport/ExportToExcel', // تأكد من أن هذا هو المسار الصحيح للـ action
            type: 'POST',
            contentType: "application/json; charset=utf-8", // تأكد من أن content-type مضبوط
            data: JSON.stringify(projectReportModel), // أرسل البيانات المحوّلة إلى JSON
            xhrFields: {
                responseType: 'blob' // مهم لاستقبال البيانات كـ Blob
            },
            success: function (blob, status, xhr) {
                // إنشاء رابط تنزيل مؤقت
                const url = window.URL.createObjectURL(blob);
                const a = document.createElement('a');
                a.href = url;

                // الحصول على اسم الملف من ترويسة Content-Disposition (إذا كانت موجودة)
                const filename = xhr.getResponseHeader('Content-Disposition')?.split('filename=')[1]?.split(';')[0]?.replace(/"/g, '');
                a.download = 'تقرير حركة مشروع.xlsx'; // استخدم الاسم من الخادم أو اسم افتراضي

                document.body.appendChild(a);
                a.click();

                // تنظيف الـ URL المؤقت
                window.URL.revokeObjectURL(url);
                document.body.removeChild(a); // إزالة الرابط من DOM
                location.reload();
            },
            error: function (xhr, status, error) {
                console.error('خطأ في تصدير البيانات:', error);
                // يمكنك هنا عرض رسالة خطأ للمستخدم
            }
        });
    });
    //========================

    $("#Filedclear").click(function (e) {
        $("#DTF").val("");
        $("#DTT").val("");
        $("#trncred").val("");
        $("#trndipt").val("");
        $("#fromDate").val("");
        $("#toDate").val("");
        $("#projectId").val(0);
        $("#trnpro").val(0);
        $("#ownerId").val(0);
    });


    //PA Report
    $("#Pdfpar").click(function () {

        var printContent = document.getElementById("PAR-to-pdf").innerHTML;
        var originalContents = document.body.innerHTML;
        // إخفاء الهيدر والفوتر
        $('header, footer').hide();
        // افتح نافذة جديدة للطباعة
        document.body.innerHTML = printContent; // ضع الجزء الذي تود طباعته فقط
        window.print(); // اطبع المحتوى
        document.body.innerHTML = originalContents; // رجع الصفحة زي ما كانت
        location.reload();
    });
    //========================
});