var insertOrUpdateFailed = false;
var JSGridHelperThatTakesAJSGridObject = (function () {
    var createJSGridTable = function (table, cssIdOfTable, koMethodForObservableArrayOfData) {
        var jsGridObject = table;

        jsGridObject = getJSGridObjectWithAbsoluteURLs(table, window.siteBaseURL);
        jsGridObject.controller = (new Controller(table, jsGridObject, koMethodForObservableArrayOfData)).getControllerObject();
        jsGridObject.fields = getFields(table);

        grid = $("#" + cssIdOfTable).jsGrid(jsGridObject);
    };

    var getJSGridObjectWithAbsoluteURLs = function (table, rootURL) {
        if (table.deleteURL !== null) {
            table.deleteURL = table.deleteURL;
        }

        if (table.insertURL !== null) {
            table.insertURL = table.insertURL;
        }

        if (table.updateURL !== null) {
            table.updateURL = table.updateURL;
        }

        return table;
    };

    var moveFromBackToFront = function (array) {
        var item = array.pop();
        array.unshift(item);
    };

    function Controller(table, jsGridObject, koMethodForObservableArrayOfData) {
        var self = this;

        self.jsGridObject = jsGridObject;

        self.koMethodForObservableArrayOfData = koMethodForObservableArrayOfData === null ? function (data) { } : koMethodForObservableArrayOfData;

        self.fields = table.fields;
        self.updateURL = table.updateURL;
        self.deleteURL = table.deleteURL;
        self.insertURL = table.insertURL;

        self.records = table.data.slice(0, table.data.length);

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
            controller.loadData = function (filter) {
                controller.filter = filter;
                return $.grep(self.records, function (record) {
                    return shouldRecordBeKeptInResults(filter, record, self.fields);
                });
            };


            if (table.editing) {
                controller.updateItem = function (item) {
                    var newItem = jsGridInsertUpdateMethod(item, self.fields, self.firstAdditionalItemProperty, self.secondAdditionalItemProperty, self.updateURL, "POST");
                    if (insertOrUpdateFailed === true) {
                        newItem = previousItem;
                    }
                    else {
                        addTempItemToRecords(previousItem, newItem, self.records);
                    }

                    self.koMethodForObservableArrayOfData(self.records);

                    return newItem;
                };
            }

            if (table.inserting) {
                controller.insertItem = function (item) {
                    var tempItem = jQuery.extend({}, item);
                    var newItem = jsGridInsertUpdateMethod(item, self.fields, self.firstAdditionalItemProperty, self.secondAdditionalItemProperty, self.insertURL, "PUT");

                    if (insertOrUpdateFailed === false) {
                        addTempItemToRecords(tempItem, newItem, self.records);
                    }

                    controller.loadData(controller.filter);

                    self.koMethodForObservableArrayOfData(self.records);

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

                    var indexOfItem = self.records.indexOf(item);
                    if (indexOfItem > -1) {
                        self.records.splice(indexOfItem, 1);
                    }

                    self.koMethodForObservableArrayOfData(self.records);

                    return item;
                };
            }

            return controller;
        };
    }

    var addTempItemToRecords = function (item, newItem, records) {
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
        var props = getPropertyNamesOfRow(record);
        var returnRecord = true;

        for (var i = 0; i < props.length; i++) {
            var currentProp = props[i].toString();
            var value = record[currentProp];
            var filterValue = filter[currentProp];

            var fieldType = fields.filter(function (field) { return field.name === currentProp }).map(function (field) { return field.type })[0];
            var isDateTimeField = fieldType === "dateTime";

            if (filterAndRecordValuesAreNotEqual(filterValue, value, isDateTimeField)) {
                if (fieldType === "text") {
                    if (!doesRecordIncludeFilterValue(value, filterValue)) {
                        returnRecord = false;
                    }
                }

                else if (isDateTimeField) {
                    returnRecord = shouldDateTimeBeKeptInResults(filterValue, value);
                }

                else {
                    returnRecord = false;
                }
            }
        }

        return returnRecord;
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
            var valueAsDate = new Date(value);

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
        //when IE is deprecated, uncomment
        //var keepDateTime = !isDateTime || (isDateTime && (filterValue.from !== null || filterValue.to !== null));
        return !isNullEmptyOrUndefined(filterValue) && recordValue !== filterValue;//add && keepDateTime when IE is deprecated
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

    var getFields = function (table) {
        fieldsAsObjects = [];
        var containsControl = false;

        for (var i = 0; i < table.fields.length; i++) {
            var field = table.fields[i];

            if (field.type === "textarea") {
                setupTextArea(field);
            }

            if (table && table.data && table.data.length > 0) {
                var firstRecord = table.data[0];
                var propNames = Object.keys(firstRecord);

                for (var j = 0; j < propNames.length; j++) {
                    if (propNames[j] === field.name && field.type !== "textarea") {
                        field.width = (field.width === null || field.width === undefined ? getFieldWidthDynamically(table.data, field) : field.width);
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
            //uncomment when ie is deprecated
            //var now = new Date();
            //this._fromPicker = $("<input>").datepicker();
            //this._toPicker = $("<input>").datepicker();
            //return $("<div>").append(this._fromPicker).append(this._toPicker);
        },

        sorter: function (date1, date2) {
            return new Date(date1) - new Date(date2);
        },

        itemTemplate: function (value) {
            if (value === null) {
                return null;
            }

            return TimeHelper.FormatEpochTimeToMonthDayYearTime(value);
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
            //uncomment when ie is deprecated
            //return {
            //    from: this._fromPicker.datepicker("getDate"),
            //    to: this._toPicker.datepicker("getDate")
            //};
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
        CreateJSGridTable: createJSGridTable
    };
})();