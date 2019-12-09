var pageMgr = Sys.WebForms.PageRequestManager.getInstance();
$(function () {
    var $reporte = $('[id*=ReportViewer]:visible')
    if ($reporte.is(':visible')) {

        ReportViewer.OnReportLoaded = function () {
            var msg = this.GetError();
            this.DisplayError(msg);
            if (!msg) {
                var reportFrame = document.getElementById(this.reportFrameID);

                var hash = reportFrame.contentWindow.document.location.hash;
                if (hash) {
                    reportFrame.contentWindow.document.location.hash = hash;
                }

                if (this.refreshDocumentMap) {
                    this.refreshDocumentMap = false;
                    var url = this.documentMapUrl + "&rnd=" + new Date();
                    this.NavigateDocumentMap(url);
                }

                var toolbar = document.getElementById(this.toolbarID);
                var pageNumberButton = null;

                var pageInfo = reportFrame.contentWindow.PageInfo;
                if ((typeof (pageInfo) != "undefined") && (pageInfo.length > 0)) {
                    this.pageCount = pageInfo[1];
                    if (toolbar) {
                        pageNumberButton = toolbar.GetButtonByName("PageNumber");

                        if (pageNumberButton) {
                            pageNumberButton.value = 1 + pageInfo[0];
                            pageNumberButton.SetPageCount(this.pageCount);
                        }
                    }
                    this.StoreCurrentPageIndex(pageInfo[0]);
                }

                this.bookmarksOnPage = reportFrame.contentWindow.BookmarksOnPage;

                if (toolbar) {
                    var paramsAreaBtn = toolbar.GetButtonByName("ParametersArea");
                    if (paramsAreaBtn) {
                        if (paramsAreaBtn.Pressed != this.parametersAreaVisible) {
                            paramsAreaBtn.ToggleState();
                        }
                    }
                }

                this.EndWait();

                if (pageNumberButton) {
                    pageNumberButton.Enable(this.pageCount > 1);
                }

                if (this.backgroundColor) {
                    var reportBody = reportFrame.contentWindow.document.getElementById('ReportBody');
                    if (reportBody) {
                        reportBody.style.backgroundColor = this.backgroundColor;
                    }
                }

                this.SetZoomInternal(toolbar);
            }

            if (hash) {
                reportFrame = document.getElementById(this.reportFrameID);
                reportFrame.contentWindow.document.location.hash = hash;
            }

            //$reporte.find('input[name*=Print]').click();
        }

        //if ($('[id*=modal]').is(':visible')) {
        //    $('[id*=modal]').modal('hide');
        //}
        //setTimeout(function () { ReportViewer.OnReportLoaded }, 3000);
    }
});