/*
Name: 			Form table transaction
Author:         Muhammad Arief 	
*/

function CurrencyLabel()
{
    var Currency = $("#CurrencyID").val();
    if(Currency=="USD"){
      var lb = "$ ";
    }else if(Currency=="IDR"){
      var lb = "Rp ";
    }else if(Currency=="YEN"){
      var lb = "¥‎ ";
    }
    return lb;
}

function getDetailField(target)
{
    var detailItem = [];
    var itemcol = $('#head-'+target+' tr');
    var itemcel = itemcol.eq(0);
    var cellCount = itemcel.find('th').length - 1;
    for (var x = 0; x <= cellCount; x++) {
      var keys = itemcol.find('th').eq(x).attr("data-col");
      var y = x-1;
      if(x==0){  }else{ detailItem[y] = keys;  }
    }
    return detailItem;
}

function getDetailItem(target) {
    var detailItem = [];
    var rowCount = document.getElementById("list-"+target).rows.length - 1;
    for (var i = 0; i <= rowCount; i++) {
        var itemcol = $('#head-'+target+' tr');
        var itemcel = $('#list-' + target + ' tr').eq(i);
        var cellCount = itemcel.find('td').length - 1;
        detailItem[i] = {};
        for (var x = 1; x <= cellCount; x++) {
            var keys = itemcol.find('th').eq(x).attr("data-col");
            var result = itemcel.find('td').eq(x).attr("data-val");
            if(keys=="UnitPrice"){ result = itemcel.find('td').eq(x).attr("data-def"); }
            if(keys=="LineTotal"){ result = accounting.unformat(result); }
            var y = x-1;
            if(x==0){  }else{ detailItem[i][keys] = result;  }
        }
    }
    return detailItem;
}

function getDetailSubItem(target) {
    var detailItem = [];
    var rowCount = document.getElementById("list-"+target).rows.length - 1;
    for (var i = 0; i <= rowCount; i++) {
        var itemcol = $('#head-'+target+' tr');
        var itemcel = $('#list-' + target + ' tr').eq(i);
        var cellCount = itemcel.find('td').length - 1;
        detailItem[i] = {};
        for (var x = 1; x <= cellCount; x++) {
            var keys = itemcol.find('th').eq(x).attr("data-col");
            var result = itemcel.find('td').eq(x).attr("data-val");
            if(keys=="UnitPrice"){ result = itemcel.find('td').eq(x).attr("data-def"); }
            if(keys=="LineTotal"){ result = accounting.unformat(result); }
            var y = x-1;
            if(x==0){  }else{ detailItem[i][keys] = result;  }
        }
        detailItem[i]['sub'] = getDetailSub(target,i);
    }
    return detailItem;
}

function getDetailSub(target,a){
    var i = a+1;
    var detailItem = [];
    var dataTable = localStorage[current_url()+"detailSub"+i];
    $('#list-detailSub').html(dataTable);
    detailItem = getDetailItem('detailSub');
    return detailItem;
}

function getDetailSubItemPOS(target) {
    var detailItem = [];
    var rowCount = document.getElementById("list-" + target).rows.length - 1;
    for (var i = 0; i <= rowCount; i++) {
        var itemcol = $('#head-' + target + ' tr');
        var itemcel = $('#list-' + target + ' tr').eq(i);
        var cellCount = itemcel.find('td').length - 1;
        detailItem[i] = {};
        for (var x = 1; x <= cellCount; x++) {
            var keys = itemcol.find('th').eq(x).attr("data-col");
            var result = itemcel.find('td').eq(x).attr("data-val");
            if (keys == "UnitPrice") { result = itemcel.find('td').eq(x).attr("data-def"); }
           // if (keys == "LineTotal") { result = accounting.unformat(result); }
            var y = x - 1;
            if (x == 0) { } else { detailItem[i][keys] = result; }
        }
        detailItem[i]['SN'] = getDetailSubPOS(target, i);
    }
    return detailItem;
}

function getDetailSubPOS(target, a) {
    var i = a + 1;
    var detailItem = $("#serialno-" + i).val();
    return detailItem.split(",");
}

function getTotal(target,type,ts) {
    var result = 0;
    var subTotal = 0;
    var res = [];
    var rowCount = document.getElementById("list-" + target).rows.length - 1;
    for (var i = 0; i <= rowCount; i++) {
        var itemcel = $('#list-' + target + ' tr').eq(i);
        subTotal = itemcel.find('td').eq(ts).attr("data-val");
        if (type == "float") {
            subTotal = parseFloat(subTotal);
            subTotal = accounting.unformat(subTotal);
            result += parseFloat(subTotal);
        }
        else if(type == "string" )
        {
            res.push(itemcel.find('td').eq(ts).attr("data-val"));
            result = res;
        }else
        {
            result += parseInt(subTotal);
        }
        var a = i+1;
        //dataDetailSub(a,target);
    }
    return result;
}

function setRowIndex(target,type,ts) {
    var result = 1;
    var subTotal = 0;
    var rowCount = document.getElementById("list-" + target).rows.length - 1;
    for (var i = 0; i <= rowCount; i++) {
        var x = i+1;
        var itemcel = $('#list-' + target + ' tr').eq(i);
        no = itemcel.find('td').eq(ts).attr("data-val");
        if(no!=x){
            result = x;
            break;
        }else{
            result = x+1;
        }
    }
    return result;
}

function adddetail(target)
{
    var valid = 1;
    var msg = '';
    var field = getDetailField(target);
    if($("#ExchRate").data("kendoNumericTextBox")){
        var ExchRate = $("#ExchRate").data("kendoNumericTextBox").value();
        var lb = CurrencyLabel();
        var Currency = $("#CurrencyID").val();
    }
    // var no = document.getElementById('list-'+target).rows.length + 1;
    var no = $("#list-" + target + " > tr:last");
    if (no.length > 0) {
        no = no.attr("id").split("-");
        no = parseInt(no[1]) + 1;
    } else { no = 1; }

    var html = '<tr id="'+target+'-'+no+'" style="background: beige;">';
    targetDo = "'"+target+"'";
    html += '<td class="actions"><span class="m-badge m-badge--primary m-badge--wide new">new</span> <input type="checkbox" class="'+target+'Check" value="'+no+'"> <a href="javascript:void(0);" onclick="editdetail('+targetDo+',' + no + ',0,1);"><i class="fa fa-pencil"></i></a><a class="delete-row" href="javascript:void(0);" onclick="removedetail('+targetDo+',' + no + ');"><i class="fa fa-trash-o"></i></a></td>';
    for (i = 0; i < field.length; i++) {
        var get = $("#"+field[i]).val();
        var txt = (get==undefined) ? "" : getTextForm(field[i]);
        var data = (get==undefined) ? "" : get;
        var dataDef = data;
        var hide = (field[i] == "RecordIDDetail" || field[i] == "RecordTimestampDetail") ? 'style="display:none;"' : "";
        if(field[i]=="Quantity"){ if(data==""){ valid = 0; msg += field[i]+" is required" + "\r\n"; } txt = accounting.formatNumber(data,2,","); }
        if(field[i]=="BeaMasuk"){ txt = accounting.formatMoney(data,lb,2,","); }
        if(field[i]=="UnitPrice"){ 
            txt = accounting.formatMoney(data,lb,2,",");
            if(Currency!="IDR" && Currency != undefined){
                if(!ExchRate){ valid = 0; msg += "ExchRate is required before add item" + "\r\n"; }
                dataDef = data * ExchRate;
                dataDef = dataDef.toFixed(2);
            } 
        }
        if(field[i]=="LineTotal"){ txt = accounting.formatMoney(data,lb,2,","); }
        if(field[i]=="Condition"){ if(data==""){ valid = 0; msg += field[i]+" is required" + "\r\n"; } }
        if(field[i]=="STS"){ data = document.getElementById("STS").checked; txt = (data) ? "&#10004;" : "-";}

        // if(get==""){ valid = 0; msg += field[i]+" is required" + "\r\n"; }
        if($("#"+field[i]).attr("datarequired") == "1"){
            if(data==""){valid = 0; msg+=field[i]+" is required"+"\r\n";}  
        }

      html += '<td id="'+target+field[i]+'v-'+no+'" data-def="'+dataDef+'" data-val="'+data+'" '+hide+'>'+txt+'</td>';

    }
    html += '</tr>';
    if (valid) {
        $('#list-'+target).append(html);
        sumTotal(target);
        sortTable(1,1,target);
        toPagination(target);
        if(target=='detailSub'){
            var RowIndex = $("#RowIndex").data("kendoNumericTextBox").value();
            var htmlUpdate = $('#list-'+target).html();
            var ID = current_url()+target+RowIndex;
            localStorage[ID] = htmlUpdate;
            // customTriger();
        }else{
            var htmlUpdate = $('#list-'+target).html();
            var ID = current_url()+target;
            localStorage[ID] = htmlUpdate;
        }
        $('#'+target+'Form').closest("[data-role=window]").kendoWindow("close");
    } else {
        var content = {};
        content.title = 'form validation';
        content.message = msg;
        $.notify(content, {
            type: 'danger',
            allow_dismiss: false,
            newest_on_top: false,
            mouse_over: false,
            showProgressbar: false,
            spacing: '10',
            timer: '2000',
            placement: {
                from: 'top',
                align: 'right'
            },
            offset: {
                x: '30',
                y: '30'
            },
            delay: '1000',
            z_index: '10000',
            animate: {
                enter: 'animated bounce',
                exit: 'animated bounce'
            }
        });
    }
}
function adddetail2(target)
{
    var valid = 1;
    var msg = '';
    var field = getDetailField(target);
    // var no = document.getElementById('list-'+target).rows.length + 1;
    var no = $("#list-" + target + " > tr:last");
    if (no.length > 0) {
        no = no.attr("id").split("-");
        no = parseInt(no[2]) + 1;
    } else { no = 1; }

    var html = '<tr id="'+target+'-'+no+'" style="background: beige;">';
    targetDo = "'"+target+"'";
    html += '<td class="actions"><span class="m-badge m-badge--primary m-badge--wide new">new</span><a href="javascript:void(0);" onclick="editdetail2('+targetDo+',' + no + ',0,1);"><i class="fa fa-pencil"></i></a><a class="delete-row" href="javascript:void(0);" onclick="removedetail('+targetDo+',' + no + ');"><i class="fa fa-trash-o"></i></a></td>';
    for (i = 0; i < field.length; i++) {
      var get = $("#"+field[i]).val();
      var txt = (get==undefined) ? "" : getTextForm(field[i]);
      var data = (get==undefined) ? "" : get;
      var hide = (field[i]=="RecordIDDetail") ? 'style="display:none;"' : "";
      if(field[i]=="Quantity"){ if(data==""){ valid = 0; msg += field[i]+" is required" + "\r\n"; } txt = accounting.formatNumber(data,2,","); }
      if(field[i]=="UnitPrice"){ txt = accounting.formatNumber(data,2,","); }
      if(field[i]=="LineTotal"){ txt = accounting.formatNumber(data,2,","); }
      if(field[i]=="Condition"){ if(data==""){ valid = 0; msg += field[i]+" is required" + "\r\n"; } }
      if(field[i]=="STS"){ data = document.getElementById("STS").checked; txt = (data) ? "&#10004;" : "-";}

      // if(get==""){ valid = 0; msg += field[i]+" is required" + "\r\n"; }
        if($("#"+field[i]).attr("datarequired") == "1"){
           if(data==""){valid = 0; msg+=field[i]+" is required"+"\r\n";}  
        }

      html += '<td id="'+target+field[i]+'v-'+no+'" data-val="'+data+'" '+hide+'>'+txt+'</td>';

    }
    html += '</tr>';
    var validator = $("#voDetailForm").data("kendoValidator");
    if (validator.validate()) {
        $('#list-'+target).append(html);
        sortTable(1,1,target);
        toPagination(target);

        var htmlUpdate = $('#Detail2').html();
        var ID = current_url();
        localStorage[ID+"Detail2"] = htmlUpdate;

        // var ID = current_url();
        // var dataStorage = localStorage[ID];
        // var dataSaveStorage;
        // if (dataStorage == undefined) {
        //     dataSaveStorage = html;
        // }else{
        //     dataSaveStorage = dataStorage + html;
        // }
        // localStorage[ID] = dataSaveStorage;

        $('#detail2Form').closest("[data-role=window]").kendoWindow("close");
    }else{
        //new PNotify({ title: "Warning", text: msg, type: 'warning', shadow: true });
        var content = {};
        content.title = "Warning";
        content.message = msg;
        $.notify(content, {
            type: 'warning',
            allow_dismiss: false,
            newest_on_top: false,
            mouse_over: false,
            showProgressbar: false,
            spacing: '10',
            timer: '2000',
            placement: {
                from: 'top',
                align: 'right'
            },
            offset: {
                x: '30',
                y: '30'
            },
            delay: '1000',
            z_index: '10000',
            animate: {
                enter: 'animated bounce',
                exit: 'animated bounce'
            }
        });
    }
}
function openWindowForm(target,id)
{
    $("#"+target+"Form").data("kendoWindow").center().open();
    cleardetail(target, 0);
    
    var no = $("#list-" + target + "-"+id+" > tr:last");
    if (no.length > 0) {
        no = no.attr("id").split("-");
        no = parseInt(no[2]) + 1;
    } else { no = 1; }

    var ItemID = $("#detailItemIDv-"+id).attr("data-val");
    var ItemName = $("#detailItemNamev-"+id).attr("data-val");

    $("#submitButtondetail2").attr("onclick","adddetail2('detail2-"+id+"')")

    $("#DPRowIndex").val(no);
    $("#DPRowIndex").data("kendoNumericTextBox").value(no);
    $("#DPItemID").val(ItemID);
    $("#DPItemName").val(ItemName);
}
function getTextForm(target)
{
    var result = "";
    if($("#"+target).data("kendoDropDownList")){
        var DDL = $("#"+target).data("kendoDropDownList");
        result = DDL.text();
    }else{
        result = $("#"+target).val();
    }
    return result;
}
function addDetailRef(DataItem,target)
{
    var html = '';
    var no = 0;
    var field = getDetailField(target);
    var targetDO = "'"+target+"'";

    for (id = 0; id < DataItem.length; id++) {
        no = id + 1;
        html += '<tr id="'+target+'-'+no+'">';
        html += '<td class="actions"><span class="m-badge m-badge--primary m-badge--wide new">new</span><a href="javascript:void(0);" onclick="editdetail('+targetDO+',' + no + ',0,1);"><i class="fa fa-pencil"></i></a><a class="delete-row" href="javascript:void(0);" onclick="removedetail('+targetDO+',' + no + ');"><i class="fa fa-trash-o"></i></a></td>';
        for (a = 0; a < field.length; a++) {
            var getfieldTable = field[a];
            var getfield = DetailTable[getfieldTable];
            var getdata = DataItem[id];
            var data = (getdata[getfield]==undefined) ? "" : getdata[getfield];
            // if(getfieldTable=="Quantity"){ data = accounting.formatNumber(data,2,","); }
            // if(getfieldTable=="UnitPrice"){ data = accounting.formatNumber(data,2,","); }
            // if(getfieldTable=="LineTotal"){ data = accounting.formatNumber(data,2,","); }

            var PackQty = getdata[DetailTable["PackQty"]];
            var QtyOutstanding = getdata[DetailTable["QtyOutstanding"]];
            if(PackQty != undefined && QtyOutstanding != undefined){
                if(getfieldTable=="PackQty"){
                    var Qty = getdata[DetailTable["Qty"]];
                    data = (PackQty/Qty)*QtyOutstanding;
                }
            }

            if(DetailTable["QtyOutstanding"] != undefined){
                if(getfieldTable=="Qty"){ data = QtyOutstanding; }
            }
            html += '<td id="'+target+getfieldTable+'v-'+no+'" data-def="'+data+'" data-val="'+data+'">'+data+'</td>';
        }
    }
    $('#list-'+target).html(html);

    if(target=='detailSub'){
        var RowIndex = $("#RowIndex").data("kendoNumericTextBox").value();
        var htmlUpdate = $('#list-'+target).html();
        var ID = current_url()+target+RowIndex;
        localStorage[ID] = htmlUpdate;
    }else{
        var htmlUpdate = $('#list-'+target).html();
        var ID = current_url()+target;
        localStorage[ID] = htmlUpdate;
    }

    sumTotal(target);
    if($("#CurrencyID").length > 0){
        var Currency = $("#CurrencyID").val();
        if(Currency != ""){
            ChangeCurrency(target,Currency);
        }
    }

    //new PNotify({ title: 'Notification', text: no+' Detail Added.', type: 'info'});
    var content = {};
    content.title = "Notification";
    content.message = no + ' Detail Added.';
    $.notify(content, {
        type: 'info',
        allow_dismiss: false,
        newest_on_top: false,
        mouse_over: false,
        showProgressbar: false,
        spacing: '10',
        timer: '2000',
        placement: {
            from: 'top',
            align: 'right'
        },
        offset: {
            x: '30',
            y: '30'
        },
        delay: '1000',
        z_index: '10000',
        animate: {
            enter: 'animated bounce',
            exit: 'animated bounce'
        }
    });
}
function autoPullData(DataItem,target)
{
    var html = '';
    var no = 0;
    var field = getDetailField(target);
    var targetDO = "'"+target+"'";

    for (id = 0; id < DataItem.length; id++) {
        no = id + 1;
        html += '<tr id="'+target+'-'+no+'">';
        html += '<td class="actions">-</a></td>';
        for (a = 0; a < field.length; a++) {
            var getfield = field[a];
            var getdata = DataItem[id];
            var data = (getdata[getfield]==undefined) ? "" : getdata[getfield];
            html += '<td id="'+target+getfield+'v-'+no+'" data-def="'+data+'" data-val="'+data+'">'+data+'</td>';
        }
    }
    $('#list-'+target).html(html);
    sumTotal("detail");
    // sumTotal(target);
    //new PNotify({ title: 'Notification', text: no+' Detail Added.', type: 'info'});
    var content = {};
    content.title = "Notification";
    content.message = no + ' Detail Added.';
    $.notify(content, {
        type: 'info',
        allow_dismiss: false,
        newest_on_top: false,
        mouse_over: false,
        showProgressbar: false,
        spacing: '10',
        timer: '2000',
        placement: {
            from: 'top',
            align: 'right'
        },
        offset: {
            x: '30',
            y: '30'
        },
        delay: '1000',
        z_index: '10000',
        animate: {
            enter: 'animated bounce',
            exit: 'animated bounce'
        }
    });
}
function editdetail(target,id,toDB) 
{
    var action = (toDB) ? "UpdatedetailDB('"+target+"'," + id + ")" : "Updatedetail('"+target+"'," + id + "); return false;";
    var field = getDetailField(target);
    for (i = 0; i < field.length; i++) {
        var get = $("#"+target+field[i]+"v-"+id).attr("data-val");
        var data = (get==undefined) ? "" : get;
        
        $("#"+field[i]).val(data);
        if($("#"+field[i]).data("kendoDropDownList")){
            $("#"+field[i]).data("kendoDropDownList").value(data);
        }
        if($("#"+field[i]).data("kendoNumericTextBox")){
            $("#"+field[i]).data("kendoNumericTextBox").value(data);
        }
        //Item Role
        // if (field[i]=="ItemType") {
        //     if (data == "NS"){
        //         $("#Qty").data("kendoNumericTextBox").enable();
        //         $("#Qty").data("kendoNumericTextBox").value(0);
        //     }else if (data == "S1") {
        //         $("#Qty").data("kendoNumericTextBox").enable(false);
        //         $("#Qty").data("kendoNumericTextBox").value(1);
        //     }else if (data == "SN") {
        //         $("#Qty").data("kendoNumericTextBox").enable();
        //         $("#Qty").data("kendoNumericTextBox").value(0);
        //     }else if (data == "SS") {
        //         $("#Qty").data("kendoNumericTextBox").enable(false);
        //         $("#Qty").data("kendoNumericTextBox").value(1);
        //     }
        // }
        //if(field[i]=="RecordFlag"){ 
        //    var RecordID = $("#RecordIDDetail").val();
        //    if(RecordID > 0){
        //        $("#"+field[i]).val(1);
        //    }
        //}
        //if(field[i]=='RowIndex'){ $("#"+field[i]).data("kendoNumericTextBox").wrapper.hide(); }
    }
    $('#list-'+target+"Sub").html('');
    var dataTable = localStorage[current_url()+target+"Sub"+id];
    if(dataTable){
        $('#list-'+target+"Sub").html(dataTable);
        customTriger(true);
    }else{
        //dataDetailSub(id,target);
    }

    $("#submitButton"+target)[0].setAttribute("onclick", action);
    $("#submitButton"+target).html('<i class="el-icon-file-edit"></i> Update');
    if($("#submitButtonOth")[0]){
        $("#submitButtonOth")[0].setAttribute("onclick", action);
        $("#submitButtonOth").html('<i class="el-icon-file-edit"></i> Update');
    }
    $("#"+target+"Form").data("kendoWindow").center().open();
}

function dataDetailSub(id,target)
{
    var RecordID = $("#RecordIDDetail").val();
    var voData = {
        RecordID: RecordID
    }; 
    var URL = "";
    if(current_module()=="Stockmovement"){ URL = site_url('Stockmovement/Beginbalance/GetSubItem')+"?LookupTranfer=1"; }else{ URL = site_url('Stockmovement/Beginbalance/GetSubItem'); }
    $.ajax({
        type: 'GET',
        data: voData,
        url:  URL,
        success: function (result) {
            if (result.errorcode > 0) {
                // new PNotify({ title: "Failed", text: result.msg, type: 'error', shadow: true });
                localStorage[current_url()+target+"Sub"+id] = "";
            } else {
                var dataTable = localStorage[current_url()+target+"Sub"+id];
                if(dataTable){
                    $('#list-'+target+"Sub").html(dataTable);
                }else{
                    localStorage[current_url()+target+"Sub"+id] = result.html;
                    $('#list-'+target+"Sub").html(result.html);
                }
                // new PNotify({ title: "Success", text: result.msg, type: 'success', shadow: true });
            }
            customTriger(true);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(jQuery.parseJSON(jqXHR.responseText));
        }
    });
}
function editdetail2(target,id,toDB) 
{
    var action = (toDB) ? "UpdatedetailDB('"+target+"'," + id + ")" : "Updatedetail2('"+target+"'," + id + ")";
    var field = getDetailField(target);
    for (i = 0; i < field.length; i++) {
        var get = $("#"+target+field[i]+"v-"+id).attr("data-val");
        var data = (get==undefined) ? "" : get;
        // console.log(data);
        $("#"+field[i]).val(data);
        if($("#"+field[i]).data("kendoDropDownList")){
            $("#"+field[i]).data("kendoDropDownList").value(data);
        }
        if($("#"+field[i]).data("kendoNumericTextBox")){
            $("#"+field[i]).data("kendoNumericTextBox").value(data);
        }
        //Item Role
        if (field[i]=="ItemType") {
            if (data == "NS"){
                $("#Qty").data("kendoNumericTextBox").enable();
                $("#Qty").data("kendoNumericTextBox").value(0);
            }else if (data == "S1") {
                $("#Qty").data("kendoNumericTextBox").enable(false);
                $("#Qty").data("kendoNumericTextBox").value(1);
            }else if (data == "SN") {
                $("#Qty").data("kendoNumericTextBox").enable();
                $("#Qty").data("kendoNumericTextBox").value(0);
            }else if (data == "SS") {
                $("#Qty").data("kendoNumericTextBox").enable(false);
                $("#Qty").data("kendoNumericTextBox").value(1);
            }
        }
        if(field[i]=="STS"){ 
            w = (data == "true");
            var STS = Boolean(w);
            $("#STS").prop("checked", STS);
        }
        //if(field[i]=='RowIndex'){ $("#"+field[i]).data("kendoNumericTextBox").wrapper.hide(); }
    }

    $('#RowIndex').attr('style','display:none');
    $("#submitButtondetail2")[0].setAttribute("onclick", action);
    $("#submitButtondetail2").html('<i class="el-icon-file-edit"></i> Update');
    if($("#submitButtonOth")[0]){
        $("#submitButtonOth")[0].setAttribute("onclick", action);
        $("#submitButtonOth").html('<i class="el-icon-file-edit"></i> Update');
    }
    $("#detail2Form").data("kendoWindow").center().open();
}

function Updatedetail(target,no) {
    var valid = 1;
    var msg = '';
    if($("#ExchRate").data("kendoNumericTextBox")){
        var ExchRate = $("#ExchRate").data("kendoNumericTextBox").value();
        var Currency = $("#CurrencyID").val();
        var lb = CurrencyLabel();
    }
    var field = getDetailField(target);
    targetDo = "'"+target+"'";
    html = '<td class="actions"><span class="m-badge m-badge--warning m-badge--wide new">updated</span> <input type="checkbox" class="'+target+'Check" value="'+no+'"> <a href="javascript:void(0);" onclick="editdetail('+targetDo+',' + no + ',0,1);"><i class="fa fa-pencil"></i></a><a class="delete-row" href="javascript:void(0);" onclick="removedetail('+targetDo+',' + no + ');"><i class="fa fa-trash-o"></i></a></td>';
    for (i = 0; i < field.length; i++) {
      var get = $("#"+field[i]).val();
      var txt = (get==undefined) ? "" : getTextForm(field[i]);
      var data = (get==undefined) ? "" : get;
      var dataDef = data;
      var hide = (field[i] == "RecordIDDetail" || field[i] == "RecordTimestampDetail") ? 'style="display:none;"' : "";
      if(field[i]=="Quantity"){ txt = accounting.formatMoney(data,lb,2,","); }
      if(field[i]=="BeaMasuk"){ txt = accounting.formatMoney(data,lb,2,","); }
      if(field[i]=="UnitPrice"){ 
            txt = accounting.formatMoney(data,lb,2,",");
            if(Currency!="IDR" && Currency != undefined){
                if(!ExchRate){ valid = 0; msg += "ExchRate is required before add item" + "\r\n"; }
                dataDef = data * ExchRate;
                dataDef = dataDef.toFixed(2);
            } 
        }
      if(field[i]=="LineTotal"){ txt = accounting.formatMoney(data,lb,2,","); }
      if(field[i]=="STS"){ data = document.getElementById("STS").checked; txt = (data) ? "&#10004;" : "-";}
      html += '<td id="'+target+field[i]+'v-'+no+'" data-def="'+dataDef+'" data-val="'+data+'" '+hide+'>'+txt+'</td>';

    }

    if(valid){
        $('#'+target+"-"+no).html(html);
        sumTotal(target);
        sortTable(1,1,target);
        toPagination(target);

         if(target=='detailSub'){
            var RowIndex = $("#RowIndex").data("kendoNumericTextBox").value();
            var htmlUpdate = $('#list-'+target).html();
            var ID = current_url()+target+RowIndex;
            localStorage[ID] = htmlUpdate;
        }else{
            var htmlUpdate = $('#list-'+target).html();
            var ID = current_url()+target;
            localStorage[ID] = htmlUpdate;
        }

        $('#'+target+'Form').closest("[data-role=window]").kendoWindow("close");
        cleardetail(target,0);
    }else{
        new PNotify({ title: "Warning", text: msg, type: 'warning', shadow: true });
    }
}

function Updatedetail2(target,no) {
    var field = getDetailField(target);
    targetDo = "'"+target+"'";
    html = '<td class="actions"><span class="m-badge m-badge--warning m-badge--wide new">updated</span><a href="javascript:void(0);" onclick="editdetail2('+targetDo+',' + no + ',0,1);"><i class="fa fa-pencil"></i></a><a class="delete-row" href="javascript:void(0);" onclick="removedetail('+targetDo+',' + no + ');"><i class="fa fa-trash-o"></i></a></td>';
    for (i = 0; i < field.length; i++) {
      var get = $("#"+field[i]).val();
      var txt = (get==undefined) ? "" : getTextForm(field[i]);
      var data = (get==undefined) ? "" : get;
      var hide = (field[i]=="RecordIDDetail") ? 'style="display:none;"' : "";
      if(field[i]=="Quantity"){ txt = accounting.formatNumber(data,2,","); }
      if(field[i]=="UnitPrice"){ txt = accounting.formatNumber(data,2,","); }
      if(field[i]=="LineTotal"){ txt = accounting.formatNumber(data,2,","); }
      if(field[i]=="STS"){ data = document.getElementById("STS").checked; txt = (data) ? "&#10004;" : "-";}
      html += '<td id="'+target+field[i]+'v-'+no+'" data-val="'+data+'" '+hide+'>'+txt+'</td>';

    }

    $('#'+target+"-"+no).html(html);
    sumTotal(target);
    sortTable(1,1,target);
    toPagination(target);
    var htmlUpdate = $('#Detail2').html();
    var ID = current_url();
    localStorage[ID] = htmlUpdate;
    $('#detail2Form').closest("[data-role=window]").kendoWindow("close");
    cleardetail(target,0);
}
function sumTotal(){

}

function cleardetail(target,id)
{
    var action = (id) ? "add"+target+"ToDB" : "adddetail";
    var field = getDetailField(target);
    var no = setRowIndex(target,"",1);
    
    // var no = document.getElementById('list-'+target).rows.length + 1;
    // var no = $("#list-" + target + " > tr:last");
    // if (no.length > 0) {
    //     no = no.attr("id").split("-");
    //     no = parseInt(no[1]) + 1;
    // } else { no = 1; }
    var RowIndex = $("input[id*='RowIndex']");
    for (i = 0; i < field.length; i++) {
        $("#"+field[i]).val("");
        if($("#"+field[i]).data("kendoDropDownList")){
            $("#"+field[i]).data("kendoDropDownList").value("");
        }
        if($("#"+field[i]).data("kendoNumericTextBox")){
            $("#"+field[i]).data("kendoNumericTextBox").value("");

            for (b = 0; b < RowIndex.length; b++) {
                var elem = $("input[id*=RowIndex]")[b].getAttribute("id");
                if(target=='detailSub'){
                    $("#RowIndex2").data("kendoNumericTextBox").value(no);
                }else{
                    $("#"+elem).data("kendoNumericTextBox").value(no);
                }
            }
        }
    }
    $('#list-'+target+"Sub").html("");
    if($("#submitButton"+target)[0]){
        $("#submitButton" + target)[0].setAttribute("onclick", action + "('" + target + "'); return false;");
        $("#submitButton"+target).html('<i class="fa fa-save"></i> &nbsp; Save');
    }
    if($("#submitButtonOth")[0]){
        $("#submitButtonOth")[0].setAttribute("onclick", action + "('"+target+"')");
        $("#submitButtonOth").html('<i class="fa fa-save"></i> &nbsp; Save');
    }
}

function removedetail(target,id) {
    var result = confirm("Want to delete ?");
    if (result) {
        removeRecordID(target,id);
        $('#'+target+'-' + id).remove();
        $('#'+target+'2-' + id).remove();

        sumTotal(target);
        toPagination(target);

        if($("div[id*=Detail].tab-pane").length > 1){
            var html = $('#Detail2').html();
            var ID = current_url();
            localStorage[ID+"Detail2"] = html;
        }
        if(target=='detail'){
            var html = $('#list-'+target).html();
            var ID = current_url()+target;
            localStorage[ID] = html;
        }else if(target=='detailSub'){
            var RowIndex = $("#RowIndex").data("kendoNumericTextBox").value();
            var html = $('#list-'+target).html();
            var ID = current_url()+target+RowIndex;
            localStorage[ID] = html;
        }
    }
}

function RemoveAll(target)
{
    var ItemTarget = $("input:checkbox[class="+target+"Check]:checked");
    if(+ItemTarget.length){
        var result = confirm("Want to delete "+ItemTarget.length+" Item ?");
        if (result) {
            ItemTarget.each(function(){
                var id = $(this).val();
                removeRecordID(target,id);
                $('#'+target+'-' + id).remove();

                sumTotal(target);
                toPagination(target);
                if(target=='detail'){
                    var html = $('#list-'+target).html();
                    var ID = current_url()+target;
                    localStorage[ID] = html;
                }else if(target=='detailSub'){
                    var RowIndex = $("#RowIndex").data("kendoNumericTextBox").value();
                    var html = $('#list-'+target).html();
                    var ID = current_url()+target+RowIndex;
                    localStorage[ID] = html;
                }
            });   
        }
    }else{
        alert("No selected data..!");
    }
}

function removeRecordID(target,id)
{
    var a = document.getElementById(target+"-"+id).rowIndex;
    var indexID = $("[data-col=RecordIDDetail]").index();
    var itemcel = $('#list-' + target + ' tr').eq(a-1);
    var RecordID = itemcel.find('td').eq(indexID).attr("data-val");
    var DR = $('#DoRemoveID'+target);
    if(DR.val()){
        DR.val(DR.val() + ',' + RecordID);
    }else{
        DR.val(RecordID);
    }
}

function CheckAll(target) {
    if(document.getElementById(target+'CheckAll').checked) {
        $('.'+target+'Check').prop('checked', true);
    }else{
        $('.'+target+'Check').prop('checked', false);
    }
}

function Calculate() {
    var qty = 0;
    if($('#Quantity').length > 0){ qty = $('#Quantity').val(); }
    if($('#Qty').length > 0){ qty = $('#Qty').val(); }

    if($("#BeaMasuk").data("kendoNumericTextBox")){
        var BeaMasuk = total * 5 / 100;
        $("#BeaMasuk").data("kendoNumericTextBox").value(BeaMasuk);
    }

    var unitPrice = $('#UnitPrice').val();
    var LineTotal = $("#LineTotal").data("kendoNumericTextBox");
    if(LineTotal != undefined){
        var total = parseFloat(qty * unitPrice);
        LineTotal.value(total);
        $('#LineTotal').val(total);
    }

    //==== Pack UOM ====//
    if($("#PackConvertion").length > 0){
        var StockSample = $("#StockSample").val();
        if(StockSample == "0"){
            var PackConvertion = $("#PackConvertion").val();
            var PackQty = parseFloat(qty/PackConvertion);
            $("#PackQty").data("kendoNumericTextBox").value(PackQty);
        }
    }
    //==== Pack UOM ====//
}

function getRumus(target)
{
    var detailItem = [];
    var itemcol = $('#'+target);
    var cellCount = itemcol.find('span').length -1;
    for (var x = 0; x <= cellCount; x++) {
      var keys = itemcol.find('span').eq(x).attr("data-val");
      detailItem[x] = keys;
    }
    return detailItem;
}

// function sort row table
function sortTable(f,n,target){
    var rows = $('#table-'+target+' tbody  tr').get();
    rows.sort(function(a, b) {

        var A = getVal(a);
        var B = getVal(b);

        if(A < B) {
            return -1*f;
        }
        if(A > B) {
            return 1*f;
        }
        return 0;
    });

    function getVal(elm){
        var v = $(elm).children('td').eq(n).text().toUpperCase();
        if($.isNumeric(v)){
            v = parseInt(v,10);
        }
        return v;
    }

    $.each(rows, function(index, row) {
        $('#table-'+target).children('tbody').append(row);
    });
}

var f_sl = 1;
$(".th-RowIndex").click(function(){
    f_sl *= -1;
    var n = $(this).prevAll().length;
    sortTable(f_sl,n,"detail");
});

// function table pagination
function toPagination(target){
    $('table#table-'+target).each(function() {
        var currentPage = 0;
        var numPerPage = 5;
        var $table = $(this);
        $(".pager").remove();
        $table.bind('repaginate', function() {
            $table.find('tbody tr').hide().slice(currentPage * numPerPage, (currentPage + 1) * numPerPage).show();
        });
        $table.trigger('repaginate');
        var numRows = $table.find('tbody tr').length;
        var numPages = Math.ceil(numRows / numPerPage);
        var $pager = $('<div class="pager"></div>');
        for (var page = 0; page < numPages; page++) {
            $('<span class="page-number"></span>').text(page + 1).bind('click', {
                newPage: page
            }, function(event) {
                currentPage = event.data['newPage'];
                $table.trigger('repaginate');
                $(this).addClass('active').siblings().removeClass('active');
            }).appendTo($pager).addClass('clickable');
        }
        $pager.insertAfter($table).find('span.page-number:first').addClass('active');
    });
}

// function for slice float
function sliceFloat(val, slc) {
    var dg = (slc == 3) ? 5 : val.lenght;
    var folat = val;
    var str = folat.toString();
    var slice = str.slice(0, dg);
    var res = parseFloat(slice);
    return res.toFixed(slc);
}

function sleep(milliseconds) {
    var start = new Date().getTime();
    for (var i = 0; i < 1e7; i++) {
        if ((new Date().getTime() - start) > milliseconds) {
            break;
        }
    }
}

//=========================== Group function for Payment ===============================//
function InsertPayment() {
    var type = parseInt($('#PaymentType').val());
    var paymentID = $('#PaymentID').val();
    var action = document.getElementById("payment-form").action;
    if (document.getElementById('PayForAmount').checked) {
        var PayFor = $('#PayForAmount').val();
    }
    if (document.getElementById('PayForWeight').checked) {
        var PayFor = $('#PayForWeight').val();
    }
    if (type == 1) {
        var voData = {
            ParentRecordID: paymentID,
            PayTypeLab: "Cash",
            PaymentType: $('#PaymentType').val(),
            MaterialID: $('#MaterialIDPym').val(),
            TransferFromAccount: $('#AccountID').val(),
            MaterialID: $('#MaterialIDPym').val(),
            DealDate: $('#paymentCashDealDate').val(),
            DealAmount: $('#paymentCash-dealAmount').val(),
            AmountPaid: $('#paymentCash-amoubtpaid').val(),
            AmountLineTotal: $('#paymentCash-total').val(),
            PayFor: PayFor,
            remarks: $('#paymentCash-desc').val()
        };
    }
    if (type == 2) {
        var voData = {
            ParentRecordID: paymentID,
            PayTypeLab: "Gold",
            PaymentType: $('#PaymentType').val(),
            MaterialID: $('#MaterialIDPym').val(),
            TransferFromAccount: $('#AccountID').val(),
            MaterialID: $('#MaterialIDPym').val(),
            DealDate: $('#paymentGoldDealDate').val(),
            Percentage: $('#paymentGold-kadar').val(),
            Weight: $('#paymentGold-barat').val(),
            dbc: $('#paymentGold-dbc').val(),
            dbw: $('#paymentGold-dbw').val(),
            AmountLineTotal: $('#paymentGold-total').val(),
            remarks: $('#paymentGold-desc').val()
        };
    }
    if (type == 3) {
        var voData = {
            ParentRecordID: paymentID,
            PayTypeLab: "Giro",
            PaymentType: $('#PaymentType').val(),
            MaterialID: $('#MaterialIDPym').val(),
            TransferFromAccount: $('#AccountID').val(),
            MaterialID: $('#MaterialIDPym').val(),
            ChequeNo: $('#paymentGiro-ChequeNo').val(),
            ChequeDate: $('#paymentGiroChequeDate').val(),
            ChequeIssuingBank: $('#paymentGiro-ChequeIssuingBank').val(),
            DealDate: $('#paymentGiroDealDate').val(),
            DealAmount: $('#paymentGiro-dealAmount').val(),
            AmountPaid: $('#paymentGiro-amoubtpaid').val(),
            AmountLineTotal: $('#paymentGiro-total').val(),
            remarks: $('#paymentGiro-desc').val()
        };
    }
    $.ajax({
        url: action,
        type: 'POST',
        data: JSON.stringify(voData),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        beforeSend: function (msg) {
            //document.getElementById("btnInvSave").disabled = true;
            //document.getElementById("btnInvSavePost").disabled = true;
        },
        success: function (result) {
            if (result.errorcode > 0) {
                new PNotify({ title: result.title, text: result.msg, type: 'error', shadow: true });
                //document.getElementById("btnInvSave").disabled = true;
                //document.getElementById("btnInvSavePost").disabled = true;
            } else {
                new PNotify({ title: result.title, text: result.msg, type: 'success', shadow: true });
                addPayment(voData, result.RecordID);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(jQuery.parseJSON(jqXHR.responseText));
        },
        async: true,
        processData: false
    });
}
function addPayment(data, no) {
    //var no = document.getElementById('list-payment').rows.length + 1;
    var html = '<tr id="payment-' + no + '"><td id="paymentTypev-' + no + '">' + data.PayTypeLab + '</td><td id="AmountLineTotalv-' + no + '">' + data.AmountLineTotal + '</td><td id="paymentItemv-' + no + '">' + data.MaterialID + '</td><td id="paymentdescv-' + no + '">' + data.remarks + '</td><td class="actions"><a href="javascript:void(0);" onclick="postPay(' + no + ');"><i class="fa fa-arrow-circle-up"></i></a><a href="javascript:void(0);" onclick="editPay(' + no + ');"><i class="fa fa-pencil"></i></a><a class="delete-row" href="javascript:void(0);" onclick="removePay(' + no + ');"><i class="fa fa-trash-o"></i></a></td></tr>'
    $('#list-payment').append(html);
}
function payCashCal() {
    var dealAmount = $('#paymentCash-dealAmount').val();
    var amoubtpaid = $('#paymentCash-amoubtpaid').val();
    if (document.getElementById('PayForAmount').checked) {
        var linetotal = parseFloat(amoubtpaid);
        var slc = 2;
    }
    if (document.getElementById('PayForWeight').checked) {
        var linetotal = parseFloat(amoubtpaid / dealAmount);
        var slc = 3;
    }
    
    $('#paymentCash-total').val(sliceFloat(linetotal, slc));
}
function payGiroCal() {
    var dealAmount = $('#paymentGiro-dealAmount').val();
    var amoubtpaid = $('#paymentGiro-amoubtpaid').val();
    var linetotal = parseFloat(amoubtpaid / dealAmount);
    $('#paymentGiro-total').val(sliceFloat(linetotal, 3));
}
function payGoldCal() {
    var kadar = $('#paymentGold-kadar').val();
    var berat = $('#paymentGold-barat').val();
    var linetotal = parseFloat(kadar / 100 / 0.995 * berat);
    $('#paymentGold-total').val(sliceFloat(linetotal, 3));
}
function SetPaymentForm(id) {
    var type = parseInt(id);
    if (type == 1) {
        $('#form-cash').show();
        $('#form-gold').hide();
        $('#form-giro').hide();
    }
    if (type == 2) {
        $('#form-cash').hide();
        $('#form-gold').show();
        $('#form-giro').hide();
    }
    if (type == 3) {
        $('#form-cash').hide();
        $('#form-gold').hide();
        $('#form-giro').show();
    }
}

function openModalPayment(item) {
    
    SetPaymentForm(item.data.PaymentType);
    var TarnsferToBankdl = $("#AccountID").data("kendoDropDownList");
    TarnsferToBankdl.select(function (dataItem) {
        return dataItem.AccountID === item.data.TransferFromAccount;
    });
    var MaterialIDdl = $("#MaterialIDPym").data("kendoDropDownList");
    MaterialIDdl.select(function (dataItem) {
        return dataItem.MaterialID === item.data.MaterialID;
    });
    $("#PaymentType").val(item.data.PaymentType);
    if (item.data.PaymentType == 1) {
        if (item.data.PayFor == 1) {
            document.getElementById('PayForAmount').checked = true;
        }
        if (item.data.PayFor == 2) {
            document.getElementById('PayForWeight').checked = true;
        }
        var datepicker = $("#paymentCashDealDate").data("kendoDatePicker");
        datepicker.value(item.DateDeal);
        var dealAmount = $("#paymentCash-dealAmount").data("kendoNumericTextBox");
        var amoubtpaid = $("#paymentCash-amoubtpaid").data("kendoNumericTextBox");
        dealAmount.value(item.data.DealAmount);
        amoubtpaid.value(item.data.AmountPaid);
        $('#paymentCash-dealAmount').val(item.data.DealAmount);
        $('#paymentCash-amoubtpaid').val(item.data.AmountPaid);
        $('#paymentCash-total').val(item.data.AmountLineTotal);
        document.getElementById("paymentCash-desc").value = item.data.Remarks;
    }
    if (item.data.PaymentType == 2) {
        var datepicker = $("#paymentGoldDealDate").data("kendoDatePicker");
        datepicker.value(item.DateDeal);
        var kadar = $("#paymentGold-kadar").data("kendoNumericTextBox");
        var barat = $("#paymentGold-barat").data("kendoNumericTextBox");
        kadar.value(item.data.Percentage);
        barat.value(item.data.Weight);
        $('#paymentGold-kadar').val(item.data.Percentage);
        $('#paymentGold-barat').val(item.data.Weight);
        $('#paymentGold-total').val(item.data.AmountLineTotal);
        document.getElementById("paymentGold-desc").value = item.data.Remarks;
    }
    if (item.data.PaymentType == 3) {
        var dateDeal = $("#paymentGiroDealDate").data("kendoDatePicker");
        var dateCheque = $("#paymentGiroChequeDate").data("kendoDatePicker");
        dateDeal.value(item.DateDeal);
        dateCheque.value(item.data.ChequeDate);
        $('#paymentGiro-ChequeNo').val(item.data.ChequeNo);
        $('#paymentGiro-ChequeIssuingBank').val(item.data.ChequeIssuingBank);
        $('#paymentGiro-ChequeIssuingBank').val(item.data.ChequeIssuingBank);
        var dealAmount = $("#paymentGiro-dealAmount").data("kendoNumericTextBox");
        var amoubtpaid = $("#paymentGiro-amoubtpaid").data("kendoNumericTextBox");
        dealAmount.value(item.data.DealAmount);
        amoubtpaid.value(item.data.AmountPaid);
        $('#paymentGiro-dealAmount').val(item.data.DealAmount);
        $('#paymentGiro-amoubtpaid').val(item.data.AmountPaid);
        $('#paymentGiro-total').val(item.data.AmountLineTotal);
        document.getElementById("paymentGiro-desc").value = item.data.Remarks;

    }
    $("#submitButtonPymt")[0].setAttribute("onclick", "UpdatePay(" + item.data.RecordID + ")");
    $("#submitButtonPymt").html('<i class="el-icon-file-edit"></i> Update');
    $("#PaymentForm").data("kendoWindow").center().open();
}

function clearPayment()
{
    var TarnsferToBankdl = $("#AccountID").data("kendoDropDownList");
    TarnsferToBankdl.select(function (dataItem) {
        return dataItem.AccountID === "";
    });
    var MaterialIDdl = $("#MaterialIDPym").data("kendoDropDownList");
    MaterialIDdl.select(function (dataItem) {
        return dataItem.MaterialID === "";
    });
    $("#PaymentType").val(1);
    document.getElementById('PayForAmount').checked = false;
    document.getElementById('PayForWeight').checked = false;
    var dateCashDeal = $("#paymentCashDealDate").data("kendoDatePicker");
    dateCashDeal.value("");
    var dealAmount = $("#paymentCash-dealAmount").data("kendoNumericTextBox");
    var amoubtpaid = $("#paymentCash-amoubtpaid").data("kendoNumericTextBox");
    dealAmount.value("");
    amoubtpaid.value("");
    $('#paymentCash-dealAmount').val("");
    $('#paymentCash-amoubtpaid').val("");
    $('#paymentCash-total').val("");
    document.getElementById("paymentCash-desc").value = "";

    var dateGoldDeal = $("#paymentGoldDealDate").data("kendoDatePicker");
    dateGoldDeal.value("");
    var kadar = $("#paymentGold-kadar").data("kendoNumericTextBox");
    var barat = $("#paymentGold-barat").data("kendoNumericTextBox");
    kadar.value("");
    barat.value("");
    $('#paymentGold-kadar').val("");
    $('#paymentGold-barat').val("");
    $('#paymentGold-total').val("");
    document.getElementById("paymentGold-desc").value = "";
   
    var dateDeal = $("#paymentGiroDealDate").data("kendoDatePicker");
    var dateCheque = $("#paymentGiroChequeDate").data("kendoDatePicker");
    dateDeal.value("");
    dateCheque.value("");
    $('#paymentGiro-ChequeNo').val("");
    $('#paymentGiro-ChequeIssuingBank').val("");
    $('#paymentGiro-ChequeIssuingBank').val("");
    var GirodealAmount = $("#paymentGiro-dealAmount").data("kendoNumericTextBox");
    var Giroamoubtpaid = $("#paymentGiro-amoubtpaid").data("kendoNumericTextBox");
    GirodealAmount.value("");
    Giroamoubtpaid.value("");
    $('#paymentGiro-dealAmount').val("");
    $('#paymentGiro-amoubtpaid').val("");
    $('#paymentGiro-total').val("");
    document.getElementById("paymentGiro-desc").value = "";

    $("#submitButtonPymt")[0].setAttribute("onclick", "InsertPayment()");
    $("#submitButtonPymt").html('<i class="el-icon-file-new"></i> Save');
}

function loadIframe(target,url)
{
    var tab = "Tab"+target;
    var frame = "frame"+target;
    // console.log(frame);
    document.getElementById(frame).src=""+url+"";
    $("#"+tab)[0].setAttribute("onclick", "javascript:void(0)");
}

function SQCal_Subtotal()
{
    var MounthlyRental = $('#MounthlyRental').val();
    var Transportation = $('#Transportation').val();
    var MiscCharge = $('#MiscCharge').val();
    var SubTotal = $("#SubTotal").data("kendoNumericTextBox");
    var total = parseInt(MounthlyRental) + parseInt(Transportation) + parseInt(MiscCharge);
    SubTotal.value(total);
    $('#SubTotal').val(total);
    SQCal_Grandtotal();
}

function SQCal_Grandtotal()
{
    var SubTotal = $('#SubTotal').val();
    var Discount = $('#Discount').val();
    var GST = $('#GST').val();
    if(GST=="GT1"){ GST = 0.07}
    if(GST=="GT2"){ GST = 0.02}
    var GrandTotal = $("#GrandTotal").data("kendoNumericTextBox");
    var GSTAmount = $("#GSTAmount").data("kendoNumericTextBox");
    var GSTN = parseFloat(GST) * parseInt(SubTotal);
    var total = parseInt(SubTotal) - parseInt(Discount) - GSTN;
    GrandTotal.value(total);
    GSTAmount.value(GSTN);
    $('#GrandTotal').val(total);
    $('#GSTAmount').val(GSTN);
}

function DateFormats(data)
{
    var monthNames = [
      "01", "02", "03",
      "04", "05", "06", "07",
      "08", "09", "10",
      "11", "12"
    ];

    var date = new Date(data);
    var day = date.getDate();
    var monthIndex = date.getMonth();
    var year = date.getFullYear();
    var result = monthNames[monthIndex]+"/"+day+"/"+year;
    // console.log(result);
    return result;
} 
function Cal_ExpiredDate() {
        $('#TerminationDate').val("");
        $('#ActualPeriod').val("");
        var DOC = $("#DOC").data("kendoDatePicker");
        var HiringPeriodDays = $("#HiringPeriodDays").data("kendoNumericTextBox");
        var HiringPeriodMounths = $("#HiringPeriodMounths").data("kendoNumericTextBox");
        var ExpiredDate = $("#ExpiredDate").data("kendoDatePicker");
        var ExpVal = ExpiredDate.value();
        var DocVal = DOC.value();
        var HRDate = HiringPeriodDays.value();
        var HRMonth = HiringPeriodMounths.value();
        var docYear = DocVal.getFullYear();
        //Cal_TotalAmount(HiringPeriodDays.value(), HiringPeriodMounths.value());
        var d = new Date(DocVal);
        d.setDate(d.getDate() + HRDate);
        d.setMonth(d.getMonth() + HRMonth);
        var Result = DateFormats(d);
        ExpiredDate.value(Result);
    }
function TermCond()
{
    var TermAndConditionID = $("#TermAndConditionID").data("kendoDropDownList");
    $('#TermAndCondition').val(TermAndConditionID.text());
}

function sleep(milliseconds) {
    var start = new Date().getTime();
    for (var i = 0; i < 1e7; i++) {
        if ((new Date().getTime() - start) > milliseconds) {
            break;
        }
    }
}
function CalculateTotal(){
    //==== Total ====//
    var SubTotal = $("#SubTotal").data("kendoNumericTextBox");
    var TotalPPN = SubTotal.value() * 0.1;
    $("#PPN").data("kendoNumericTextBox").value(TotalPPN);

    var Total = SubTotal.value() + TotalPPN;
    $("#Total").data("kendoNumericTextBox").value(Total);
    //==== Total ====//

    //==== Grand Total ====//
    var Discount = $("#Discount").data("kendoNumericTextBox");
    var MiscCharges = $("#MiscCharges").data("kendoNumericTextBox");
    
    var GrandTotal = Total - Discount.value() + MiscCharges.value();
    
    $("#GrandTotal").data("kendoNumericTextBox").value(GrandTotal);
    //==== Grand Total ====//
}
function StockSampleChange(){
    var StockSample = $("#StockSample").val();
    var RowIndex = $("#RowIndex").val();
    var btnAttr = $("#submitButtondetail").attr("onclick");
    var btnHtml = $("#submitButtondetail").html();
    cleardetail("detail",0);
    $("#submitButtondetail").attr("onclick",btnAttr);
    $("#submitButtondetail").html(btnHtml);
    $("#StockSample").data("kendoDropDownList").value(StockSample);
    $("#RowIndex").data("kendoNumericTextBox").value(RowIndex);
}