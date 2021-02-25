function gridViewBasic(data)
{ 
	alert("gridViewBasic");
    var x = data.tools.length;
    var prm = "";
    for (i = 0; i < x; i++) { prm += " \"#="+data.tools[i]+"#\", "; };
    var defcolumn = [
        {
           "width":data.actBTN,
            "template":"<a class='k-button'  onclick='actButton([\"Edit\", "+ prm +" ])' href='javascript:void(0)'><span class='k-icon k-edit'></span></a>"
            +"<a class='k-button' onclick='actButton([\"Delete\", "+ prm +" ])' href='javascript:void(0)'><span class='k-icon k-delete'></span></a>"
            +"<a class='k-button'  onclick='actButton([\"Detail\", "+ prm +" ])' href='javascript:void(0)'><span class='k-icon k-i-hbars'></span></a>",
        }

    ];
    var column = defcolumn.concat(data.columnz);
    //console.log(column);
    $("#"+data.id).kendoGrid({
        toolbar: [
        {
            template:"<a class='k-button' onclick='actButton()' href='javascript:void(0)'><span class='k-icon k-add'></span>Add New</a>"
        },
        {
            name:"excel",
            fileName: " "+data.id+".xlsx"
        }
        ],
        height: data.height,
        width: "100%",
        columns: column,
        dataSource: {
            transport: {
                read: {
                    type:"GET",
                    data: { table: data.table },
                    url: site_url(data.url.read),
                    dataType: "json"
                },
            },
            sync: function(e) {
                $("#"+data.id).data('kendoGrid').dataSource.read();
                $("#"+data.id).data('kendoGrid').refresh();
            },
            schema: {
                data: function(data){
                    return data.data;
                },
                total: function(data){
                    return data.count;
                },
                model: {
                    id: data.table+"r001",
                }
            },
            pageSize: 10, serverPaging: true,  serverFiltering: true,  serverSorting: true,
        },
        filterable: true, sortable: true, pageable: true, groupable: true, resizable: true, selectable: true, scrollable: true, reorderable:true,
        filterable: { mode: "row", },
        pageable: {
            refresh: true,
            pageSizes: true,
            buttonCount: 5
        },
    });

//Close Modal
    $("#noButtonDelete").click(function(){
        $('#ActDelete').closest("[data-role=window]").data("kendoWindow").close();
    });

    $("#noButtonDetail").click(function(){
        $('#ActDetail').closest("[data-role=window]").data("kendoWindow").close();
    });

    $("#noButtonAdd").click(function(){
        $('#ActAdd').closest("[data-role=window]").data("kendoWindow").close();
    });

    $("#noButtonEdit").click(function(){
        $('#ActEdit').closest("[data-role=window]").data("kendoWindow").close();
    });

    $("#yesButtonDelete").click(function(){
            var voData = {
               RecordID: $('#RecordID_Delete').val()
           };

           $.ajax({
            type: 'POST',
            data: voData,
            url:  site_url(data.url.destroy),
            success: function (result) {
                $('#ActDelete').closest("[data-role=window]").data("kendoWindow").close();
                $("#"+data.id).data('kendoGrid').dataSource.read();
                $("#"+data.id).data('kendoGrid').refresh();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert(jQuery.parseJSON(jqXHR.responseText));
            }
        });
    });

//Delete Modal KendoWindow
    $("#ActDelete").kendoWindow({
        width: "400px",
        title: "Want to delete ?",
        visible: false,
        modal: true,
        actions: [
        "Close"
        ],
    });

    $("#ActDetail").kendoWindow({
        width: "400px",
        title: "Detail",
        visible: false,
        modal: true,
        actions: [
        "Close"
        ],
    });

    $("#ActEdit").kendoWindow({
        width: "400px",
        title: "Edit",
        visible: false,
        modal: true,
        actions: [
        "Close"
        ],
    });

    $("#ActAdd").kendoWindow({
        width: "400px",
        title: "Add",
        visible: false,
        modal: true,
        actions: [
        "Close"
        ],
    });

    var x;
    var DList = data.DList;
    for (x in DList) {
        dropDownList(x,DList[x]);
    }

}

function onlyGridView(data)
{
	alert("onlyGridView");
    var x = data.tools.length;
    var prm = "";
    var printUrl = "'"+data.url.form+"Print'";
    for (i = 0; i < x; i++) { prm += " \"#="+data.tools[i]+"#\", "; };
    var defcolumn = [
        {
            "width":data.actBTN,
            "template":
            ''
            +'# if( '+data.table+'r003==1)' 
            +'{#<a class="k-button" href="'+site_url(data.url.form+"Detail/#="+data.table+"r001#")+'"><span class="k-icon k-i-hbars"></span></a><a class="k-button" href="javascript:void(0)" onclick="openTabPrint('+printUrl+',#='+data.table+'r001#)"><span class="fa fa-print"></span></a>#}'
            +'else'
            +'{#<a class="k-button k-button-icontext k-grid-edit" href="'+site_url(data.url.form+"/#="+data.table+"r001#")+'"><span class="k-icon k-edit"></span></a>'
            +"<a class='k-button k-button-icontext  k-grid-delete' onclick='actButton([\"Delete\", "+ prm +" ])' href='javascript:void(0)'><span class='k-icon k-delete'></span></a>"
            +'<a class="k-button" href="'+site_url(data.url.form+"Detail/#="+data.table+"r001#")+'"><span class="k-icon k-i-hbars"></span></a>'
            +'<a class="k-button" href="javascript:void(0)" onclick="openTabPrint('+printUrl+',#='+data.table+'r001#)"><span class="fa fa-print"></span></a># }#',
        },
        {
            "width":data.postBTN,
            "template":
            "# if( "+data.table+"r003==0) {#<a class='k-button k-button-icontext k-grid-edit' onclick='actButton([\"Post\", "+ prm +" ])' href='javascript:void(0)'>P</span></a># } else {#<a class='k-button k-button-icontext k-grid-edit' onclick='actButton([\"UnPost\", "+ prm +" ])' href='javascript:void(0)'>U</span></a># }#",

        }
    ];
    if(data.postBTN=="0"){ 
        var defcolumn = [
            {
                "width":data.actBTN,
                "template":
                ''
                +'# if( '+data.table+'r003==1)' 
                +'{#<a class="k-button" href="'+site_url(data.url.form+"Detail/#="+data.table+"r001#")+'"><span class="k-icon k-i-hbars"></span></a><a class="k-button" href="javascript:void(0)" onclick="openTabPrint('+printUrl+',#='+data.table+'r001#)"><span class="fa fa-print"></span></a>#}'
                +'else'
                +'{#<a class="k-button k-button-icontext k-grid-edit" href="'+site_url(data.url.form+"/#="+data.table+"r001#")+'"><span class="k-icon k-edit"></span></a>'
                +"<a class='k-button k-button-icontext  k-grid-delete' onclick='actButton([\"Delete\", "+ prm +" ])' href='javascript:void(0)'><span class='k-icon k-delete'></span></a>"
                +'<a class="k-button" href="'+site_url(data.url.form+"Detail/#="+data.table+"r001#")+'"><span class="k-icon k-i-hbars"></span></a>'
                +'<a class="k-button" href="javascript:void(0)" onclick="openTabPrint('+printUrl+',#='+data.table+'r001#)"><span class="fa fa-print"></span></a># }#',
            }
        ];
    }
    if(data.actBTN=="0"){ 
        var defcolumn = [];
    }
    var column = defcolumn.concat(data.columnz);
    $("#"+data.id).kendoGrid({
        toolbar: [
        {     
            template:
            '<a class="k-button k-button-icontext k-grid-add" href="'+site_url(data.url.form)+'"><span class="k-icon k-add"></span>Add New</a>'
        },
        {
            name:"excel",
            fileName: "stockbalance.xlsx"
        }
        ],
        height: data.height,
        width: "100%",
        columns: column,
        dataSource: {
            transport: {
                read: {
                    type:"GET",
                    data: { table: data.table, customfilter: data.customfilter},
                    url: site_url(data.url.read),
                    dataType: "json"
                },
            },
            sync: function(e) {
                $("#"+data.id).data('kendoGrid').dataSource.read();
                $("#"+data.id).data('kendoGrid').refresh();
            },
            schema: {
                data: function(data){
                    return data.data;
                },
                total: function(data){
                    return data.count;
                },
                model: {
                    id: data.table+"r001",
                }
            },
            pageSize: 10, serverPaging: true,  serverFiltering: true,  serverSorting: true,
        },
        filterable: true, sortable: true, pageable: true, groupable: true, resizable: true, selectable: true, scrollable: true, reorderable:true,
        filterable: { mode: "row", },
        pageable: {
            refresh: true,
            pageSizes: true,
            buttonCount: 5
        },
    });

    $("#noButtonDelete").click(function(){
        $('#ActDelete').closest("[data-role=window]").data("kendoWindow").close();
    });

    $("#noButtonDetail").click(function(){
        $('#ActDetail').closest("[data-role=window]").data("kendoWindow").close();
    });

    $("#noButtonPost").click(function(){
        $('#ActPost').closest("[data-role=window]").data("kendoWindow").close();
    });

    $("#noButtonUnPost").click(function(){
        $('#ActUnPost').closest("[data-role=window]").data("kendoWindow").close();
    });

    $("#yesButtonDelete").click(function(){
            var voData = {
               RecordID: $('#RecordID_Delete').val()
           };

           $.ajax({
            type: 'POST',
            data: voData,
            url:  site_url(data.url.destroy),
            success: function (result) {
                $('#ActDelete').closest("[data-role=window]").data("kendoWindow").close();
                $('#'+data.id).data('kendoGrid').dataSource.read();
                $('#'+data.id).data('kendoGrid').refresh();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert(jQuery.parseJSON(jqXHR.responseText));
            }
        });
    });

    $("#yesButtonPost").click(function(){
        var voData = {
            ID: $('#RecordID_Post').val(),
            DocNo : $('#DocNo_Post').val()
        };

        $.ajax({
            type: "POST",
            data: voData,
            url:  site_url(data.url.post),
             success: function (result) {
                if (result.errorcode > 0) {
                    new PNotify({ title: "Failed", text: result.msg, type: "warning", shadow: true });
                } else {
                    new PNotify({ title: "Posted", text: result.msg, type: "success", shadow: true });
                    $('#ActPost').closest("[data-role=window]").data("kendoWindow").close();
                    $('#'+data.id).data('kendoGrid').dataSource.read();
                    $('#'+data.id).data('kendoGrid').refresh();
                }
           },
           error: function (jqXHR, textStatus, errorThrown) {
               alert(jQuery.parseJSON(jqXHR.responseText));
           }
        });
    });

    $("#yesButtonUnPost").click(function(){
        var voData = {
            ID: $('#RecordID_UnPost').val(),
            DocNo : $('#DocNo_UnPost').val()
        };

        $.ajax({
            type: "POST",
            data: voData,
            url:  site_url(data.url.unpost),
             success: function (result) {
                PNotify.removeAll();
                if (result.errorcode > 0) {
                    new PNotify({ title: "Failed", text: result.msg, type: "warning", shadow: true });
                } else {
                    new PNotify({ title: "UnPosted", text: result.msg, type: "success", shadow: true });
                    $('#ActUnPost').closest("[data-role=window]").data("kendoWindow").close();
                    $('#'+data.id).data('kendoGrid').dataSource.read();
                    $('#'+data.id).data('kendoGrid').refresh();
                }
           },
           error: function (jqXHR, textStatus, errorThrown) {
               alert(jQuery.parseJSON(jqXHR.responseText));
           }
        });
    });

    $("#ActDelete").kendoWindow({
        width: "400px",
        title: "Want to delete?",
        visible: false,
        modal: true,
        actions: [
        "Close"
        ],
    });

    $("#ActDetail").kendoWindow({
        width: "400px",
        title: "Detail",
        visible: false,
        modal: true,
        actions: [
        "Close"
        ],
    });

    $("#ActPost").kendoWindow({
        width: "400px",
        title: "Post",
        visible: false,
        modal: true,
        actions: [
        "Close"
        ],
    });

    $("#ActUnPost").kendoWindow({
        width: "400px",
        title: "Post",
        visible: false,
        modal: true,
        actions: [
        "Close"
        ],
    });

    //DP List Format
    function DPList()
    {

    }
}

function GridListHeaderDetail(data)
{   
	alert("GridListHeaderDetail");
    console.log(data);
    var column = data.columnz;
    $("#"+data.id).kendoGrid({
        toolbar: [
        {
            name:"excel",
            fileName: data.excelname
        }],
        height: data.height,
        width: "100%",
        columns: column,
        detailTemplate: kendo.template($("#template").html()),
        detailInit: function(e){
            var detailRow = e.detailRow;
            var detailfilter = data.detail.customfilter;
            var customfilter = {};
            for(var object in detailfilter) {                
                var ab = detailfilter[object];
                if(ab.indexOf("<VALUE>") != -1){
                    ab = ab.replace("<VALUE>","");
                    customfilter[object] = ab;
                }
                if(ab.indexOf("<HEADER>") != -1){
                    ab = ab.replace("<HEADER>","");
                    customfilter[object] = e.data[ab];
                }
            }
            detailRow.find("."+data.detail.id).kendoGrid({
                dataSource: {
                    transport: {
                        read: {
                            type:"GET",
                            data: { table: data.detail.table, customfilter: customfilter},
                            url: site_url(data.detail.url.read),
                            dataType: "json"
                        },
                    },            
                    schema: {
                        data: function(detail){
                            return detail.data;
                        },
                        total: function(detail){
                            return detail.countDetail;
                        }
                    },
                    pageSize: 10,
                    serverPaging: true,
                    serverFiltering: true,
                    serverSorting: true
                },
                filterable: true,
                sortable: true,
                pageable: true,
                groupable: true,
                resizable: true,
                selectable: true,
                scrollable: true,
                reorderable:true,
                filterable: {
                    mode: "row",
                },
                pageable: {
                    refresh: true,
                    pageSizes: true,
                    buttonCount: 5
                },
                columns: data.detail.column
            });
        },
        dataSource: {
            transport: {
                read: {
                    type:"GET",
                    data: { table: data.table, customfilter: data.customfilter},
                    url: site_url(data.url.read),
                    dataType: "json"
                },
            },
            sync: function(e) {
                $("#"+data.id).data('kendoGrid').dataSource.read();
                $("#"+data.id).data('kendoGrid').refresh();
            },
            schema: {
                data: function(data){
                    return data.data;
                },
                total: function(data){
                    return data.count;
                },
                model: {
                    id: data.table+"r001",
                }
            },
            pageSize: 10, serverPaging: true,  serverFiltering: true,  serverSorting: true,
        },
        filterable: true, sortable: true, pageable: true, groupable: true, resizable: true, selectable: true, scrollable: true, reorderable:true,
        filterable: { mode: "row", },
        pageable: {
            refresh: true,
            pageSizes: true,
            buttonCount: 5
        },
    });
}
function GridListDetail(e,test){
    console.log(e);
    
}
function gridMasterA(data)
{ 
	alert("gridMasterA");
    var defcolumn = [
        {
            "width":"30px",
            "command":[
            {
                "name":"edit",
                "buttonType":"ImageAndText",
                "text":" "
            },
            {
                "name": "Delete",  
                "buttonType":"ImageAndText",
                "text":"<span class='k-icon k-delete'></span>",
                click: function(e){  
                    e.preventDefault(); 
                    var tr = $(e.target).closest("tr"); 
                    var dataGrid = this.dataItem(tr);
                    var confirm = "#"+data.id+"confirm";
                    var confirmTamplate = "#"+data.id+"confirmTemplate";
                    var title = 'Are you sure to delete this record?';
                    var window = $(confirm).kendoWindow({
                        title: title,
                        visible: false, 
                        modal: true,
                        width: "310px",
                        height: "180px",
                    }).data("kendoWindow");
                    $('.k-window-title').text(title);
                    var windowTemplate = kendo.template($(confirmTamplate).html());
                    window.content(windowTemplate(dataGrid)); 
                    window.center().open();
                    var grid = this;
                    $("#yesButton").click(function(){
                        grid.dataSource.remove(dataGrid)  
                        grid.dataSource.sync()
                        window.close();
                    })
                    $("#noButton").click(function(){
                        window.close();
                    })
                }                              
            }
            ]
        }

    ];
    var column = defcolumn.concat(data.columnz);
    $("#"+data.id).kendoGrid({
        toolbar: [ { name:"create"},{ name:"excel", fileName: "GeneralList.xlsx" } ],
        height: data.height,
        columns: column,
        groupable: true,
        sortable: true,
        selectable: true,
        scrollable: true,
        filterable: {
            mode: "row",
        },
        pageable: {
            refresh: true,
            pageSizes: true,
            buttonCount: 5
        },
        editable:
        {
            template: kendo.template($("#custom_popup").html()),
            "confirmation":"Are you sure you want to delete this record?",
            "confirmDelete":"Delete",
            "window": {width: "350px"},
            "destroy":true,
            "create":true,
            "update":true,
            "mode":"popup"
        },
        edit: function(e) {
            //dropDownList();
            var x;
            var DList = data.DList;
            for (x in DList) {
                console.log(DList);
                dropDownList(x,DList[x]);
            }
            if (e.model.isNew()) {
                $('.k-window-title').text('Add New');
                $('.k-grid-update').html('<span class="k-icon k-update"></span> Save');
            }
            else {
                $('input[name="NameCode"]').attr('readonly',true);
            }
        },
        dataSource: {
            transport: {
                read: {
                    url: site_url(data.url.read)+'?table='+data.table,
                    dataType: "json"
                },
                update: {
                    url: site_url(data.url.update),
                    type: "POST",
                    dataType: "json"
                },
                destroy: {
                    url: site_url(data.url.destroy),
                    data: {RecordID: "#=RecordID#"},
                    type: "POST",
                    dataType: "json"    
                },
                create: {
                    url: site_url(data.url.create),
                    type: "POST",
                    dataType: "json"
                }
            },
            pageSize: 20,
            schema: {
                data: function(datas){
                    return datas.data;
                },
                total: function(datas){
                    return datas.length;
                },
                model: {
                    id: "RecordID",
                    fields: data.FieldUse
                }
            },
        }
    });
}

function dropDownList(index,item)
{
	alert("dropDownList");
    $("#"+item.text).removeClass("k-textbox");
    $("#"+item.text).kendoDropDownList({
        dataTextField: item.key,
        dataValueField: index,
        filter: "contains",
        optionLabel: "Select",
        dataSource: {
            transport: {
                read: {
                    url: site_url(item.url),
                    dataType: "json"
                }
            },
            schema: {
                data: function(data){
                    return data.data;
                }
            }
        }
    });
    $("#"+item.text+"_Edit").removeClass("k-textbox");
    $("#"+item.text+"_Edit").kendoDropDownList({
        dataTextField: item.key,
        dataValueField: index,
        filter: "contains",
        optionLabel: "Select",
        dataSource: {
            transport: {
                read: {
                    url: site_url(item.url),
                    dataType: "json"
                }
            },
            schema: {
                data: function(data){
                    return data.data;
                }
            }
        }
    });
}

function gridViewGeneral(data)
{ 
	alert("gridViewGeneral");
    var defcolumn = [
        {
            "width":"15px",
            "template":"<a class='k-button' href='"+site_url(data.url.form+'/#=RecordID#')+"'><span class='fa fa-list'></span></a>"
        },
        {
            "width":"15px",
            "command":[
            {
                "name": "Delete",  
                "buttonType":"ImageAndText",
                "text":"<span class='k-icon k-delete'></span>",
                click: function(e){  
                    e.preventDefault(); 
                    var tr = $(e.target).closest("tr"); 
                    var dataGrid = this.dataItem(tr);
                    var confirm = "#"+data.id+"confirm";
                    var confirmTamplate = "#"+data.id+"confirmTemplate";
                    var title = 'Are you sure to delete this record?';
                    var window = $(confirm).kendoWindow({
                        title: title,
                        visible: false, 
                        modal: true,
                        width: "400px",
                        height: "200px",
                    }).data("kendoWindow");
                    $('.k-window-title').text(title);
                    var windowTemplate = kendo.template($(confirmTamplate).html());
                    window.content(windowTemplate(dataGrid)); 
                    window.center().open();
                    var grid = this;
                    $("#yesButton").click(function(){
                        grid.dataSource.remove(dataGrid)  
                        grid.dataSource.sync()
                        window.close();
                    })
                    $("#noButton").click(function(){
                        window.close();
                    })
                }                              
            }
            ]
        },
        {
            "width":"15px",
            "command":[
                { text: "P",name:"post", click: PostData },
                { text: "U",name:"unpost", click: UnPostData }
            ]
        }

    ];
    var column = defcolumn.concat(data.columnz);
    $("#"+data.id).kendoGrid({
        toolbar: [ { template:
		            '<a class="k-button k-button-icontext k-grid-add" href="'+site_url(data.url.form)+'"><span class="k-icon k-add"></span>Add New</a>'},{ name:"excel", fileName: "GeneralList.xlsx" } ],
        height: data.height,
        columns: column,
        groupable: true,
        sortable: true,
        selectable: true,
        scrollable: true,
        dataBound: onDataBound,
        filterable: {
            mode: "row",
        },
        pageable: {
            refresh: true,
            pageSizes: true,
            buttonCount: 5
        },
        editable:
        {
            "confirmation":"Are you sure you want to delete this record?",
            "confirmDelete":"Delete",
            "window": {width: "350px"},
            "destroy":true,
            "create":false,
            "mode":"popup"
        },
        edit: function(e) {
            if (e.model.isNew()) {
                $('.k-window-title').text('Add New');
                $('.k-grid-update').html('<span class="k-icon k-update"></span> Save');
            }
            else {
                $('input[name="NameCode"]').attr('readonly',true);
            }
        },
        dataSource: {
            transport: {
                read: {
                    url: site_url(data.url.read),
                    dataType: "json"
                },
                update: {
                    url: site_url(data.url.update),
                    type: "POST",
                    dataType: "json"
                },
                destroy: {
                    url: site_url(data.url.destroy),
                    data: {RecordID: "#=RecordID#"},
                    type: "POST",
                    dataType: "json"    
                },
                create: {
                    url: site_url(data.url.create),
                    type: "POST",
                    dataType: "json"
                }
            },
            pageSize: 20,
            schema: {
                data: function(datas){
                    return datas.data;
                },
                total: function(datas){
                    return datas.length;
                },
                model: {
                    id: "RecordID",
                    //fields: data.FieldUse
                }
            },
        }
    });
}

function openTabPrint(url,id) {
    var win = window.open(site_url(url+"/"+id+"?print=true"), '_blank');
    win.focus();
}