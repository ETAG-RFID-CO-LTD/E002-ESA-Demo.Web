$(document).ready(function () {
    ItemInfoHelper.GenerateItemInfoGrid();
});

var ItemInfoManager = {
    gridDataSource: function () {
        var gridDataSource = new kendo.data.DataSource({
            type: "json",
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            allowUnsort: true,
            pageSize: 10,
            transport: {
                read: {
                    type: "POST",
                    dataType: "json",
                    contentType:"application/json;charset=utf-8"
                },
                parameterMap: function (options) {
                    return JSON.stringify(options);
                }
            },
            schema: {
                data: "Items", total: "TotalCount",
                model: {
                    fields: {
                        items: {
                            defaultValue: {ItemId:0, ItemName:"---Select---"}
                        }
                    }
                }
            }
        });
        return gridDataSource;
    }

    SaveItemInfo: function () {
        var itemObj = ItemInfoHelper.CreateItemList();
         
    }
};

var ItemInfoHelper = {
    GenerateItemInfoGrid: function () {
        $('#grdItemInfo').kendoGrid({
            dataSource: ItemInfoManager.gridDataSource(),
            pagable: false,
            editable: {
                createAt: "bottom",
                mode: "incell"
            },
            toolbar: [{ name: "create", text: "Add New" }],
            filterable: false,
            sortable: false,
            columns: ItemInfoHelper.GenerateItemColumn(),
            navigatable: true,
            selectable: "row"
        });
    },

    GenerateItemColumn: function () {
        return columns = [
            { field: "ID", hidden: true },
            { field: "Items", title: "Item", editor: ItemInfoHelper.ItemDropDownEditor, template: "#=item.ItemName#" },
            { field: "Qnty", title: "Qnty" ,width:50 },
            { field: "Rate", title: "Rate", width: 50  },
            { field: "Amt", title: "Amt", width: 50 }
        ];
    },

    ItemDropDownEditor: function (container, options) {
        $('<input required data-text="ItemName" data-value-field="ItemId" data-bind="value:' + options.field + '"/>')
            .appendTo(container)
            .kendoDropDownList({
                autoBind: false,
                optionLabel: '--Select--',
                dataSource: [
                    { ItemId: 1, ItemName: "Item-1" },
                    { ItemId: 1, ItemName: "Item-2" },
                    { ItemId: 1, ItemName: "Item-3" },
                    { ItemId: 1, ItemName: "Item-4" }
                ],
                placeholder:"Please Select"
            });
    }
};