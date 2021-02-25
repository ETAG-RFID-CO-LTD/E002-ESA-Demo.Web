$(document).ready(function(){
    $(".KendoDatePicker").kendoDatePicker({format:"MM/dd/yyyy"});
   
    if($("#CurrencyID").val()==="IDR")
    {
        $(".Kurs").addClass("hide");
    }else{
        $(".Kurs").removeClass("hide");
    }
    //Doc Status Dropdown 
    var data = [
        {text: "CANCEL", value:"-2"},
        {text: "UNPOST", value:"-1"},
        {text: "DRAFT", value:"0"},
        {text: "POSTED", value:"1"}
    ];
    //$("#DocStatus").kendoDropDownList({
    //    dataTextField: "text",
    //    dataValueField: "value",
    //    dataSource: data,
    //    optionLabel: "Select"
    //});
    //lsOnLoad();

    $("#form-insert").bind('submit',function(e){
        e.preventDefault();
        if (validasi === "insert") {
            insert();
        }else{
            update();
        };
    });

    $("#DetailModal").click(function() {
        $("#detailForm").data("kendoWindow").center().open();
        cleardetail("detail", 0);
    });

    $("#Detail2Modal").click(function() {
        $("#detail2Form").data("kendoWindow").center().open();
        cleardetail("detail2", 0);
    });

    $("#Detail3Modal").click(function() {
        $("#detail3Form").data("kendoWindow").center().open();
        cleardetail("detail3", 0);
    });

});

var dialog = $("#dialog");

function onClose(e) {
    mApp.unblockPage();
    $("#grid").data("kendoGrid").dataSource.read();
}

function onCancel(e) {
    //DBLoad(); 
    mApp.unblockPage();
    lsClear();
    $("#grid").data("kendoGrid").dataSource.read();
}

function onOK(e) {
    //RecoveryLoad();   
    mApp.unblockPage();
}

function checkDuplicateInObject(propertyName, inputArray) {
    var seenDuplicate = false,
        testObject = {};
  
    inputArray.map(function(item) {
        var itemPropertyName = item[propertyName]; 
        if (itemPropertyName in testObject) {
            testObject[itemPropertyName].duplicate = true;
            item.duplicate = true;
            seenDuplicate = true;
        }
        else {
            testObject[itemPropertyName] = item;
            delete item.duplicate;
        }
    });
  
    return seenDuplicate;
}

function RecoveryLoad(){    
    if(typeof(Storage) !== "undefined") {  
        if(LSTable.length > 0){
            var len = LSTable.length - 1;
            for (var i = 0; i <= len; i++) {
                if(ID==""){
                    $('#'+LSTable[i].tbodyID).html(localStorage[LSTable[i].lsID]);
                }else{
                    if(localStorage[LSTable[i].lsID]){
                        var dataTable = localStorage[LSTable[i].lsID];
                    }
                    $('#'+LSTable[i].tbodyID).html(dataTable);  
                }
                sortTable(1,1,LSTable[i].detailPrefix);
                toPagination('detail');
            }
        }
    }
}
function DBLoad(){    
    if(typeof(Storage) !== "undefined") {  
        if(LSTable.length > 0){
            var len = LSTable.length - 1;
            for (var i = 0; i <= len; i++) {
                if(ID==""){

                }else{
                    var dataTable = LSTable[i].element;
                    $('#'+LSTable[i].tbodyID).html(dataTable);
                    
                }
                sortTable(1,1,LSTable[i].detailPrefix);
                toPagination('detail');
            }
        }
    }
}
function lsOnLoad(){    
    if(typeof(Storage) !== "undefined") {  
        if(LSTable.length > 0){
            var len = LSTable.length - 1;
            for (var i = 0; i <= len; i++) {
                if(ID==""){
                    if(localStorage[LSTable[i].lsID]){
                        $("#dialog").data("kendoDialog").open();
                    }
                }else{
                    if(localStorage[LSTable[i].lsID]){
                        $("#dialog").data("kendoDialog").open();
                        //var dataTable = localStorage[LSTable[i].lsID];
                        //var html = '<span id="doc-temp" class="doc active" onclick="OpenDoc(1);">Temporary</span> | <span id="doc-ori" class="doc" onclick="OpenDoc(0);">Original</span>';
                        //$("#panel-doc").html(html);
                    }else{
                        var dataTable = LSTable[i].element;
                        $('#'+LSTable[i].tbodyID).html(dataTable);    
                    }
                    
                }
                sortTable(1,1,LSTable[i].detailPrefix);
                toPagination('detail');
            }
        }
    }
}
function lsClear(){
    var localData = JSON.parse(localStorage[KEY]);
    localStorage.clear(KEY);
    //alert("ISClear");
    //alert(localData);
    //for(var i=0; i<localData.length; i++){
    //    localStorage.removeItem(KEY + "Sub-" + localData[i].ItemID);
    //}
    ////if(LSTable.length > 0){
    ////    var len = LSTable.length - 1;
    ////    for (var j = 0; j <= len; i++) {
    ////        localStorage.removeItem(LSTable[j].lsID);
    ////        localStorage.removeItem(LSTable[j].lsID+"-Doremove");
    ////       // localStorage.removeItem(LSTable[j].lsID+"-DoremoveSub");
    ////    }
    ////}
}
function CalculateHeader()
{
    sumTotal("detail");
}

function OpenDoc(active)
{
    var len = LSTable.length - 1;
    for (var i = 0; i <= len; i++) {
        if(active){
            $('#'+LSTable[i].tbodyID).html(localStorage[LSTable[i].lsID]);
            document.getElementById("doc-ori").className = "doc";
            document.getElementById("doc-temp").className = "doc active";
        }else{
            $('#'+LSTable[i].tbodyID).html(LSTable[i].element);
            document.getElementById("doc-ori").className = "doc active";
            document.getElementById("doc-temp").className = "doc";
        }
        sumTotal(LSTable[i].detailPrefix);
        toPagination('detail');
    }
}

// Semasi Finish Good Script //
function changeColor(colorID){
	var color = "btn-danger";
	switch (colorID) {
		case "CL1":
			color = "btn-danger";
			break;
		case "CL2":
			color = "btn-success";
			break;
		case "CL3":
			color = "btn-warning";
			break;
		default:
			color = "btn-default";
			break;
	}

	return color;
}

function getRackItem() {
     var Rack = [];
     if($("#RackTable").length > 0){
         var rowCount = document.getElementById("RackTable").rows.length - 1;
         var cellCount = document.getElementById("RackTable").rows.item(0).cells.length;
         var locid = $("#LocationID").val();
         var racid = $("#RackID").val();
         var rkl = 0;
         for (var i = 0; i <= rowCount; i++) {
             var n = i + 1;
             for (var x = 0; x <= cellCount; x++) {
                 var nc = x + 1;
                 var idbtn = locid + "-" + racid + "-R" + n + "C" + nc;
                 var btnRackID = idbtn;
                 var btnRack = $("#" + btnRackID).html();
                 var rowcount = $("#hdRowCount" + btnRackID).val();
                 var columnCount = $("#hdCelCount" + btnRackID).val();
                 var color = $("#hdColor" + btnRackID).val();
                 var MaxCapacity = $("#hdMaxCapacity" + btnRackID).val();
                 if (btnRack != null/* && btnRack != "_____"*/) {
                     Rack[rkl] = {};
                     Rack[rkl]["ColumnID"] = btnRackID;
                     Rack[rkl]["ColumnName"] = btnRack;
                     Rack[rkl]["RowNo"] = n;
                     Rack[rkl]["ColumnNo"] = nc;
                     Rack[rkl]["Color"] = color;
                     Rack[rkl]["MaxCapacity"] = MaxCapacity;
                     Rack[rkl]["ParentRackID"] = $("#RackID").val();

                     rkl++;
                 }
             }
         }
     }
     return Rack;
 }

function EditColumn(e) {
     $("#hdIdCol").val(e).change();
     var desc = $("#" + e).html();
     $("#tbColName").val(desc).change();
     $("#ChangeTable #submitButtonOth").attr("onclick","ChangeValCol('"+e+"');");
     $("#ChangeTable").data("kendoWindow").center().open();

     var MaxCapacity = $("#hdMaxCapacity" + e).val();
     $("#tbMaxCapacity").data("kendoNumericTextBox").value(MaxCapacity);
     var Color = $("#hdColor" + e).val();
     $("#tbColor").data("kendoDropDownList").value(Color);

     // CreateTableRack();
     var LocData;
     var column = $("#HdCell").val();
     if (column != "") {
         LocData = JSON.parse(column);
     }
     else
         LocData = "";

     if (LocData != "") {

         for (var item in LocData) {
             if (LocData[item].RackID == e) {
                 for (var x in LocData[item].Column) {

                     $("#CR" + LocData[item].Column[x].RowNo + "C" + LocData[item].Column[x].ColumnNo).html(LocData[item].Column[x].ColumnName).change();
                 }
             }
         }

     }

     //clearItem(0);
 }
function ChangeValCol(id) {
     var colid = $("#hdIdCol").val();
     var colname = $("#tbColName").val();
     var color = $("#tbColor").val();
     var maxcapacity = $("#tbMaxCapacity").data("kendoNumericTextBox").value();

     var btnColor = changeColor(color);

     // $("#" + colid).removeAttr("class");
     $("#" + colid).attr("class","btn btn-rack "+btnColor);

     $("#" + colid).html(colname);
     $("#hdColor"+id).val(color);
     $("#hdColumnID"+id).val(colid);
     $("#hdColumnName"+id).val(colname);
     $("#hdMaxCapacity"+id).val(maxcapacity);

     CloseModal("ChangeTable");
 }
function CloseModal(id) {
    $("#"+id).data("kendoWindow").close();
    //clearItem(0);
}
function CreateTable() {
     var RC = $('#RowCounts').val();
     var CC = $('#ColumnCount').val();
     var RI = $('#RackID').val();
     var LI = $('#LocationID').val();
     var DCVal = $('#DefaultColor').val();

     var DC = changeColor(DCVal);
     var msg = "";

     if(RI === ""){msg += "RackID is required \n";}
     if(RC === ""){msg += "Row count is required \n";}
     if(CC === ""){msg += "Column count is required \n";}
     if(LI === ""){msg += "Location is required \n";}
     if(DCVal === ""){msg += "Default Color is required";}

     if(msg != ""){
        new PNotify({ title: "Form Validation", text: msg, type: 'error', shadow: true });
     }else{
         var table = "<table id='RackTable' class='table'>";
         for (i = 1; i <= RC; i++) {
             table += "<tr>";
             for (o = 1; o <= CC; o++) {

                 var idbtn = LI + "-" + RI + "-R" + i + "C" + o;
                 var id = '"' + idbtn + '"';
                 var button = "<button type='button' class='btn-rack btn "+DC+"' id='" + idbtn + "' onclick='EditColumn(" + id + ");'>Row "+ i +" Col "+ o +"</button>";
                 var RawNo = "<input type='hidden' id='hdRowNo" + idbtn + "' value='" + i + "'  />";
                 var CelNo = "<input type='hidden' id='hdCelNo" + idbtn + "' value='" + o + "'  />";
                 var ColumnID = "<input type='hidden' id='hdColumnID" + idbtn + "' />";
                 var Color = "<input type='hidden' class='hdColor' id='hdColor" + idbtn + "' value='"+DCVal+"' />";
                 var ColumnName = "<input type='hidden' id='hdColumnName" + idbtn + "' />";
                 var MaxCapacity = "<input type='hidden' id='hdMaxCapacity" + idbtn + "' />";
                 table += "<td>" + button + Color + RawNo + CelNo + ColumnID + ColumnName + MaxCapacity + "</td>";

             }
             table += "</tr>";
         }
         table += "</table>";
         document.getElementById("content").innerHTML = table;

         var VoRackColumn = [];
         var locrack = $("#HdRack").val();
         var LocData = JSON.parse(locrack);

         if (LocData != null) {
             if (LocData.LocationID != null) {
                 $('#LocationID').prop('disabled', true).addClass('k-state-disabled');
                 $('#RecordID').val(LocData.RecordID).change();
                 $('#SRecordTimestamp').val(LocData.SRecordTimestamp).change();
                 $('#RecordStatus').val(LocData.RecordStatus).change();
                 $('#ListCode').val(LocData.ListCode).change();
             }
             var nx = 0;
             for (var item in LocData) {
                var itemColor = changeColor(LocData[item].Color);
                 $("#"+LocData[item].ColumnID).html(LocData[item].ColumnName);
                 $("#"+LocData[item].ColumnID).attr('class','btn-rack btn '+ itemColor);
                 $("#hdColor"+LocData[item].ColumnID).val(LocData[item].Color);
                 $("#hdRowCount" + LocData[item].ColumnID).val(LocData[item].ColumnCount).change();
                 $("#hdCelCount" + LocData[item].ColumnID).val(LocData[item].RowCounts).change();
                 $("#hdColumnID"+ LocData[item].ColumnID).val(LocData[item].ColumnID).change();
                 $("#hdColumnName"+ LocData[item].ColumnID).val(LocData[item].ColumnName).change();
                 $("#hdMaxCapacity"+ LocData[item].ColumnID).val(LocData[item].MaxCapacity).change();
             }
             if( JSON.stringify(VoRackColumn) !== "[]")
                $("#HdCell").val(JSON.stringify(VoRackColumn)).change();

         }
     }
}
function kendoModal(id,title,width="550px")
{

    $("#"+id).kendoWindow({
        width: width,
        title: title,
        modal:true,
        visible: false,
        actions: [
        "Close"
        ],
    });
}

function filterBrand() {
	var location = $("#BrandID").data("kendoDropDownList").value();
	$("#BrandName").val(brand).change();
}

function filterVendor() {
	var location = $("#VendorID").data("kendoDropDownList").value();
	$("#VendorName").val(vendor).change();
}

 function filterLocation(){
    var location = $("#LocationID").data("kendoDropDownList").value();
    $("#LocationName").val(location).change();
}

function filterRole() {
	var Role = $("#RoleID").data("kendoDropDownList").value();
	$("#RoleName").val(Role).change();
}

 function alphanumeric(target){
    $('#'+target).keypress(function (e) {
        var regex = new RegExp("^[a-zA-Z0-9]+$");
        var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
        if (regex.test(str)) {
            return true;
        }

        e.preventDefault();
        return false;
    });
    // $('#'+target).keyup(function(){
    //     var strVal = $('#'+target).val();
    //     $('#'+target).val(strVal.toUpperCase());
    // });
 }
 function defaultColor(){
 	var DC = $("#DefaultColor").val();
     var color = changeColor(DC);

 	$('.btn-rack').attr('class','btn btn-rack '+color);
 	$('.hdColor').val(DC);
 	// $('.btn-rack').addClass('btn btn-rack .'+DC);
 }

function printLayoutCarton(data){
    var start = '';
    var end = '';
    var data_print = '';

    start +=
    '<table width="100%" border="1" cellpadding="5" cellspacing="0">'+
    '<tr>'+
    '<th style=text-align:center>No</th>'+
    '<th style=text-align:center>RFID Code</th>'+
    '<th style=text-align:center>No Carton</th>'+
    '<th style=text-align:center>Shipping Date</th>'+
    '<th style=text-align:center>Location</th>'+
    '</tr>';
    for (var i =0; i<data.data.length; i++) {
        no = i+1;
        data_print += '<tr>'+
                      '<td align="center">' + no + '</td>'+
                      '<td align="center">' + data.data[i].RFIDCode + '</td>'+
                      '<td align="center">' + data.data[i].no_carton + '</td>'+
                      '<td align="center">' + data.data[i].shipping_date + '</td>'+
                      '<td align="center">' + data.data[i].location + '</td>' +
                      '</tr>';
    }
    end += '</table>';

    $('#print').html(start + data_print + end);
}

function printLayoutCartonDetail(elm){
    var start_detail    = '';
    var dataDetail      = '';
    var end_detail      = '';

    var dataID = elm.getAttribute('data-id');
    $.ajax({
        url: site_url("information/searching_rack/list_rack_detail"),
        type: 'POST',
        data: {
            "dataID":dataID
        },
        success:function(data){

            start_detail +=
            '<table width="100%" border="0" cellpadding="5" cellspacing="0">' +
                '<tr>' +
                    '<td width="20%" align="center">RFID Code :</td>' +
                    '<td colspan="2" width="30%" align="center">'+data.data[0].RFIDCode+'</td>' +
                    '<td width="20%" align="center">Shiping Date :</td>' +
                    '<td width="30%" align="center">'+data.data[0].shipping_date+'</td>' +
                '</tr>' +
                '<tr>' +
                    '<td width="20%" align="center">No Carton :</td>' +
                    '<td colspan="2" width="20%">'+data.data[0].no_carton+'</td>' +
                    '<td width="20%" align="center">Location :</td>' +
                    '<td width="20%" align="center">'+data.data[0].location+'</td>' +
                '</tr>' +
            '</table>' +
            '<table  width="100%" border="1" cellpadding="5" cellspacing="0" style="margin-top:2%">' +
                '<tr>' +
                    '<td align="center">No</td>' +
                    '<td align="center">PRODUCT TICKET</td>' +
                    '<td align="center">CEL</td>' +
                    '<td align="center">CO</td>' +
                    '<td align="center">JOB</td>' +
                    '<td align="center">PO</td>' +
                '</tr>';
                for (var i =0; i<data.data.length; i++) {
                    no = i+1;
                    dataDetail += '<tr>'+
                                    '<td align="center">'+ no +'</td>'+
                                    '<td>'+ data.data[i].ticket_num +'</td>'+
                                    '<td align="center">'+ data.data[i].UF_GS01_ProductionCell +'</td>'+
                                    '<td align="center">'+ data.data[i].co_num +'</td>'+
                                    '<td align="center">'+ data.data[i].job +'</td>'+
                                    '<td align="center">'+ data.data[i].cust_po +'</td>'+
                                    '</tr>';
                }
            end_detail += '</table>';

            $('#printDetail_rak').html(start_detail + dataDetail + end_detail);
            PrintElem("#printDetail_rak");
        },
        error: function (jqXHR, textStatus, errorThrown)
        {
            alert('Error Search Data');
        }
    });
}

function PrintElem(elem)
{
    Popup($(elem).html());
}

function Popup(data)
{
    var mywindow = window.open('', 'Data Karton', 'height=500,width=800');
    mywindow.document.write('<html><head><title>Data Karton</title>');
    /*optional stylesheet*/ //mywindow.document.write('<link rel="stylesheet" href="main.css" type="text/css" />');
    mywindow.document.write('</head><body >');
    mywindow.document.write(data);
    mywindow.document.write('</body></html>');

    mywindow.document.close(); // necessary for IE >= 10
    mywindow.focus(); // necessary for IE >= 10

    mywindow.print();
    mywindow.close();

    return true;
}

$(function () {
    var container = $("#voDetailForm");
    kendo.init(container);
    container.kendoValidator({
        rules: {
            validmask: function (input) {
                if (input.is("[data-validmask-msg]") && input.val() != "") {
                    var maskedtextbox = input.data("kendoMaskedTextBox");
                    return maskedtextbox.value().indexOf(maskedtextbox.options.promptChar) === -1;
                }

                return true;
            }
        }
    });
});