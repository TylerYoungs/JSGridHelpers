var insertOrUpdateFailed = false;

var JSGridHelperThatTakesAJSGridObject = (function () {
    var jsGridHelper = this;
    jsGridHelper.getTimeMethod = TimeHelper.FormatEpochTimeToMonthDayYearTime;

    var createJSGridTable = function (table, cssIdOfTable, koMethodForObservableArrayOfData, koMethodForJSGridObject) {
        var jsGridObject = table;
        koMethodForObservableArrayOfData = koMethodForObservableArrayOfData === null || koMethodForObservableArrayOfData === undefined ? ko.observableArray([]) : koMethodForObservableArrayOfData;

        jsGridObject = table;
        jsGridObject.controller = (new controller(table, jsGridObject, koMethodForObservableArrayOfData)).getControllerObject();
        jsGridObject.fields = getFields(table, koMethodForObservableArrayOfData());

        if (koMethodForJSGridObject !== null && koMethodForJSGridObject !== undefined) {
            koMethodForJSGridObject(jsGridObject);
        }

        getJquerySelectorForTable(cssIdOfTable).jsGrid(jsGridObject);
    };

    var getJquerySelectorForTable = function (cssIdOfTable) {
        return $("#" + cssIdOfTable);
    }

    var refresh = function (cssIdOfTable) {
        getJquerySelectorForTable(cssIdOfTable).jsGrid("refresh");
    }

    var moveFromBackToFront = function (array) {
        var item = array.pop();
        array.unshift(item);
    };

    var controller = function (table, jsGridObject, koMethodForObservableArrayOfData) {
        var self = this;

        self.jsGridObject = jsGridObject;
        self.getURL = table.getURL;
        self.getData = table.getData;

        self.koMethodForObservableArrayOfData = koMethodForObservableArrayOfData;
        self.koMethodForObservableArrayOfData(self.koMethodForObservableArrayOfData().slice(0, self.koMethodForObservableArrayOfData().length));

        self.fields = table.fields;
        self.updateURL = table.updateURL;
        self.deleteURL = table.deleteURL;
        self.insertURL = table.insertURL;

        self.firstAdditionalItemProperty = table.firstAdditionalItemProperty;
        self.secondAdditionalItemProperty = table.secondAdditionalItemProperty;

        table.onItemUpdating = function (args) {
            previousItem = args.previousItem;
        };

        table.onItemInserted = function (args) {
            if (insertOrUpdateFailed === false) {
                moveFromBackToFront(this.data);
            }
        };

        self.getControllerObject = function () {

            var controller = {};

            controller.filter = {};

            var getDeferredDataFromKOObservable = function (d, filter, fields) {
                var items = $.grep(self.koMethodForObservableArrayOfData(), function (record) {
                    return shouldRecordBeKeptInResults(filter, record, fields);
                });

                self.koMethodForObservableArrayOfData(items);
                var expectedObj = { data: items, itemsCount: [items.length] };

                d.resolve(expectedObj);
            };

            controller.loadData = function (filter) {
                controller.filter = filter;

                var d = $.Deferred();

                if (self.getURL !== null && self.getURL !== undefined && self.getData !== null && self.getData !== undefined) {
                    var localGetData = self.getData;
                    if (filter.sortField && filter.sortOrder) {
                        self.getData.ColumnSort = { Name: filter.sortField, Order: filter.sortOrder };
                        localGetData = $.param(self.getData).replace(/%5b([^0-9].*?)%5d/gi, '.$1');
                    }

                    $.get(self.getURL, localGetData).done(function (data) {
                        self.koMethodForObservableArrayOfData(data);
                        getDeferredDataFromKOObservable(d, filter, self.fields);
                    });
                }
                else {
                    getDeferredDataFromKOObservable(d, filter, self.fields);
                }

                return d.promise();
            };


            if (table.editing) {
                controller.updateItem = function (item) {
                    var newItem = jsGridInsertUpdateMethod(item, self.fields, self.firstAdditionalItemProperty, self.secondAdditionalItemProperty, self.updateURL, "POST");
                    if (insertOrUpdateFailed === true) {
                        newItem = previousItem;
                    }
                    else {
                        self.koMethodForObservableArrayOfData(addTempItemToAndGetRecords(previousItem, newItem, self.koMethodForObservableArrayOfData()));
                    }

                    return newItem;
                };
            }

            if (table.inserting) {
                controller.insertItem = function (item) {
                    var tempItem = jQuery.extend({}, item);
                    var newItem = jsGridInsertUpdateMethod(item, self.fields, self.firstAdditionalItemProperty, self.secondAdditionalItemProperty, self.insertURL, "PUT");

                    if (insertOrUpdateFailed === false) {
                        self.koMethodForObservableArrayOfData(addTempItemToAndGetRecords(tempItem, newItem, self.koMethodForObservableArrayOfData()));
                    }

                    controller.loadData(controller.filter);

                    return newItem;
                };
            }

            if (table.deleting) {
                controller.deleteItem = function (item) {
                    var formattedItem = getItemFormattedForDelivery(item, self.fields, self.firstAdditionalItemProperty, self.secondAdditionalItemProperty);

                    $.ajax({
                        url: self.deleteURL,
                        type: "DELETE",
                        data: formattedItem
                    }).done(function (deletedItem) {
                    });

                    var indexOfItem = self.koMethodForObservableArrayOfData().indexOf(item);
                    if (indexOfItem > -1) {
                        self.koMethodForObservableArrayOfData(self.koMethodForObservableArrayOfData().splice(indexOfItem, 1));
                    }

                    return item;
                };
            }

            return controller;
        };
    }

    var addTempItemToAndGetRecords = function (item, newItem, records) {
        delete records[item];

        var index = records.indexOf(item);
        var itemsToRemove = 0;

        if (index === -1) {
            index = 0;
        }
        else {
            itemsToRemove = 1;
        }

        records.splice(index, itemsToRemove, newItem);

        return records;
    };

    var jsGridInsertUpdateMethod = function (item, fields, firstAdditionalItemProperty, secondAdditionalItemProperty, url, httpType) {
        var formattedItem = getItemFormattedForDelivery(item, fields, firstAdditionalItemProperty, secondAdditionalItemProperty);

        $.ajax({
            url: url,
            type: httpType,
            data: formattedItem,
            async: false
        }).done(function (itemFromDB) {
            if (itemFromDB !== null && itemFromDB !== undefined) {
                formattedItem = itemFromDB;
                insertOrUpdateFailed = false;
            }
            else {
                insertOrUpdateFailed = true;
            }
        }).fail(function () {
            insertOrUpdateFailed = true;
        });

        return formattedItem;
    };

    var getArrayFromArrayFunction = function (arrayToCopy) {
        var tempArray = [];

        for (var j = 0; j < arrayToCopy.length; j++) {
            tempArray.push(arrayToCopy[j]);
        }

        return tempArray;
    };

    var getItemFormattedForDelivery = function (item, fields, firstAdditionalItemProperty, secondAdditionalItemProperty) {
        var tempItem = jQuery.extend({}, item);

        var multiSelectFields = _.where(fields, { type: "multiSelect" });

        for (var i = 0; i < multiSelectFields.length; i++) {
            var selectedItems = tempItem[multiSelectFields[i].name];
            tempItem[multiSelectFields[i].name] = getArrayFromArrayFunction(selectedItems);
        }

        if (firstAdditionalItemProperty !== undefined && firstAdditionalItemProperty !== null) {
            tempItem.FirstAdditionalItemProperty = firstAdditionalItemProperty;
        }

        if (secondAdditionalItemProperty !== undefined && secondAdditionalItemProperty !== null) {
            tempItem.SecondAdditionalItemProperty = secondAdditionalItemProperty;
        }

        return tempItem;
    };

    var shouldRecordBeKeptInResults = function (filter, record, fields) {
        var propCountOfFilter = 0;
        $.each(filter, function (key, value) { propCountOfFilter++; });

        if (propCountOfFilter === 0 && filter.constructor === Object) {
            return true;
        }

        var props = getPropertyNamesOfRow(record);

        for (var i = 0; i < props.length; i++) {
            var currentProp = props[i].toString();
            var value = record[currentProp];
            var filterValue = filter[currentProp];

            var fieldType = fields.filter(function (field) { return field.name === currentProp }).map(function (field) { return field.type })[0];
            var isDateTimeField = fieldType === "dateTime";

            if (filterAndRecordValuesAreNotEqual(filterValue, value, isDateTimeField)) {

                if (fieldType === "text") {
                    if (!doesRecordIncludeFilterValue(value, filterValue)) {
                        return false;
                    }
                }

                else if (isDateTimeField && !shouldDateTimeBeKeptInResults(filterValue, value)) {
                    return false;
                }
            }
        }

        return true;
    };

    var shouldDateTimeBeKeptInResults = function (filterValue, value) {
        var from = filterValue.from;
        var to = filterValue.to;

        var isFromNullOrEmpty = isNullOrEmpty(from);
        var isToNullOrEmpty = isNullOrEmpty(to);
        var isValueNullOrEmpty = isNullOrEmpty(value);


        if (isFromNullOrEmpty && isToNullOrEmpty) {
            return true;
        }

        else if (!isValueNullOrEmpty) {
            valueAsDate = new Date(jsGridHelper.getTimeMethod(value));

            if (!isFromNullOrEmpty && !isToNullOrEmpty && valueAsDate < to && valueAsDate > from) {
                return true;
            }

            else if (!isToNullOrEmpty && isFromNullOrEmpty && valueAsDate < to) {
                return true;
            }

            else if (!isFromNullOrEmpty && isToNullOrEmpty && valueAsDate > from) {
                return true;
            }
        }

        return false;
    };

    var isNullOrEmpty = function (value) {
        return value === null || value === "";
    };

    var isNullEmptyOrUndefined = function (value) {
        return isNullOrEmpty(value) || value === undefined;
    };

    var filterAndRecordValuesAreNotEqual = function (filterValue, recordValue, isDateTime) {
        var keepDateTime = !isDateTime || (isDateTime && (filterValue.from !== null || filterValue.to !== null));
        return !isNullEmptyOrUndefined(filterValue) && recordValue !== filterValue && keepDateTime;
    };

    var doesRecordIncludeFilterValue = function (recordValue, filterValue) {
        return recordValue !== null && recordValue.toString().toLowerCase().indexOf(filterValue.toString().toLowerCase()) !== -1;
    };

    var getPropertyNamesOfRow = function (obj) {
        var returnArray = [];
        $.each(obj, function (key, value) { returnArray.push(key); });

        return returnArray;
    };

    var setupTextArea = function (field) {
        field.editTemplate = function (value) {
            var textArea = $("<textarea>");

            textArea.addClass('textAreaInJSGrid');
            textArea.val(value);

            return this._textArea = textArea;
        };

        field.editValue = function () {
            return this._textArea.val();
        };
    };

    var getFields = function (table, data) {
        fieldsAsObjects = [];
        var containsControl = false;

        for (var i = 0; i < table.fields.length; i++) {
            var field = table.fields[i];

            if (field.type === "textarea") {
                setupTextArea(field);
            }

            if (table && data && data.length > 0) {
                var firstRecord = data[0];
                var propNames = Object.keys(firstRecord);

                for (var j = 0; j < propNames.length; j++) {
                    if (propNames[j] === field.name && field.type !== "textarea") {
                        field.width = field.width === null || field.width === undefined ? getFieldWidthDynamically(data, field) : field.width;
                        break;
                    }
                }
            }

            if (field.type.toLowerCase().indexOf("button") !== -1) {
                field.headerTemplate = $("<p>").text(field.title);
                delete field.title;
            }

            if (field.type === "control") {
                containsControl = true;
            }

            fieldsAsObjects.push(field);
        }

        if (containsControl === false) {
            fieldsAsObjects.push(getFinalControl(table.editing, table.deleting));
        }

        return fieldsAsObjects;
    };

    var getFinalControl = function (editing, deleting) {
        return {
            type: "control",
            modeSwitchButton: false,
            editButton: editing,
            deleteButton: deleting
        };
    };

    var getFieldWidthDynamically = function (rows, field) {
        if (field.type !== 'hidden') {
            var titleWords = field.title.split(" ");

            var NUMBEROFPIXELSPERCHAR = 10;
            var MAXLENGTH = 0;
            var DATETIMEFORMATTEDLENGTH = 15;

            for (var i = 0; i < titleWords.length; i++) {
                var titleWordLength = titleWords[i].length;
                MAXLENGTH = titleWordLength > MAXLENGTH ? titleWordLength : MAXLENGTH;
            }

            var propName = field.name;
            if (field.type === 'dateTime') {
                MAXLENGTH = DATETIMEFORMATTEDLENGTH > MAXLENGTH ? DATETIMEFORMATTEDLENGTH : MAXLENGTH;
            }
            else {
                for (var j = 0; j < rows.length; j++) {
                    if (rows[j][propName] !== null) {
                        var propValueLength = rows[j][propName].toString().length;
                        MAXLENGTH = propValueLength > MAXLENGTH ? propValueLength : MAXLENGTH;
                    }
                }
            }

            return MAXLENGTH * NUMBEROFPIXELSPERCHAR + "px";
        }

        else {
            return "0px";
        }
    };

    var DateTimeField = function (config) {
        jsGrid.Field.call(this, config);
    };

    var MultiselectField = function (config) {
        jsGrid.Field.call(this, config);
    };

    var originalFilterTemplate = jsGrid.fields.text.prototype.filterTemplate;
    jsGrid.fields.text.prototype.filterTemplate = function () {
        var grid = this._grid;
        var $result = originalFilterTemplate.call(this);

        $result.on("keyup", function (e) {
            grid.search();
        });

        return $result;
    };

    DateTimeField.prototype = new jsGrid.Field({
        css: "date-field",
        align: "center",

        filterTemplate: function () {
            this._fromPicker = $("<input>").datepicker();
            this._toPicker = $("<input>").datepicker();
            return $("<div>").append(this._fromPicker).append(this._toPicker);
        },

        sorter: function (date1, date2) {
            var date1 = jsGridHelper.getTimeMethod(date1);
            var date2 = jsGridHelper.getTimeMethod(date2);

            return new Date(date1) - new Date(date2);
        },

        itemTemplate: function (value) {
            if (value === null) {
                return null;
            }
            return jsGridHelper.getTimeMethod(value);
        },

        insertTemplate: function (value) {
            return this._insertPicker = $("<input>").datepicker();
        },

        editTemplate: function (value) {
            return this._editPicker = $("<input>").datepicker().datepicker("setDate", new Date(value));
        },

        insertValue: function () {
            return this._insertPicker.datepicker("getDate").toISOString();
        },

        editValue: function () {
            return this._editPicker.datepicker("getDate").toISOString();
        },

        filterValue: function () {
            return {
                from: this._fromPicker.datepicker("getDate"),
                to: this._toPicker.datepicker("getDate")
            };
        }
    });

    MultiselectField.prototype = new jsGrid.Field({

        items: [],
        textField: "",
        valueField: "",

        itemTemplate: function (values) {
            var self = this;

            self.items = this.items;
            self.valueField = this.valueField;
            self.textField = this.textField;

            var newValues = getArrayFromArrayFunction(values);

            var selectedItems = self.items.filter(function (item) {
                return newValues.includes(item[self.valueField]);
            });

            selectedItemIds = selectedItems.map(function (item) { return item[self.textField] });

            return $.makeArray(selectedItemIds).join(", ");
        },

        _createSelect: function (selected) {
            var textField = this.textField;
            var valueField = this.valueField;

            var $result = $("<select>").prop("multiple", true);

            $.each(this.items, function (_, item) {
                var text = item[textField];
                var value = item[valueField];

                var $opt = $("<option>").text(text);
                $opt.attr("value", value);

                if ($.inArray(value, selected) > -1) {
                    $opt.attr("selected", "selected");
                }

                $result.append($opt);
            });

            return $result;
        },

        insertTemplate: function () {
            var insertControl = this._insertControl = this._createSelect();

            return insertControl;
        },

        editTemplate: function (value) {
            var editControl = this._editControl = this._createSelect(value);

            return editControl;
        },

        insertValue: function () {
            return this._insertControl.find("option:selected").map(function () {
                return this.selected ? $(this).attr("value") : null;
            });
        },

        editValue: function () {
            return this._editControl.find("option:selected").map(function () {
                return this.selected ? $(this).attr("value") : null;
            });
        }

    });

    var origFinishInsert = jsGrid.loadStrategies.DirectLoadingStrategy.prototype.finishInsert;
    jsGrid.loadStrategies.DirectLoadingStrategy.prototype.finishInsert = function (insertedItem) {

        if (insertOrUpdateFailed === true) {
            return;
        }

        origFinishInsert.apply(this, arguments);
    };

    jsGrid.fields.dateTime = DateTimeField;
    jsGrid.fields.multiSelect = MultiselectField;

    return {
        Create: createJSGridTable,
        Refresh: refresh,
    };
})();