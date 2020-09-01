import 'core-js';
import * as React from "react"
import * as ReactDOM from "react-dom"
import { Remote } from "../../Common/Remote/Remote"
import { Input } from "@progress/kendo-react-inputs";
import { Button } from "@progress/kendo-react-buttons";
import { MultiSelect } from '@progress/kendo-react-dropdowns';
import { filterBy } from '@progress/kendo-data-query';
declare var window: any;
declare var $: any;

interface IMultiSelectProps {
    dataUrl: string
}

interface IMultiSelectState {
    value: any[],
    singleValue: any,
    sourceValue: any[],
    isTooltip: boolean,
    mouseEvent: any;
    dataList: any[];
}

export class MultiSelectDropDown extends React.Component<IMultiSelectProps, IMultiSelectState> {
    dataList: any[];
    scrollPosition: any;
    constructor(props: any) {
        super(props);

        this.onBindTooltip = this.onBindTooltip.bind(this);
        this.onOpen = this.onOpen.bind(this);
        this.onClose = this.onClose.bind(this);

        this.state = {
            value: [],
            sourceValue: [],
            singleValue: '',
            isTooltip: false,
            mouseEvent: null,
            dataList: [],
        };
    }

    getUsersEmail() {
        let sender = this;
        Remote.get(this.props.dataUrl,
            (response: any) => {
                sender.dataList = response.data;
                sender.setState({
                    sourceValue: response.data,
                });
            },
            (err: any) => { window.Dialog.alert(err) });
    }

    onChange = (event: any) => {
        this.setState({
            value: event.target.value,
            mouseEvent: event
        }, this.forceUpdate);

        //close tooltip after element is removed from multiselect dropdown..
        $(".distribution-list-user").remove();
        this.onBindTooltip();
    }

    componentDidMount() {
        this.getUsersEmail();
    }

    onBindTooltip() {
        let sender = this;
        setTimeout(() => {
            $(".k-multiselect-wrap").find(".k-button").each((i: number, c: any) => {
                $(c).off('mouseover');
                $(c).on("mouseover",
                    (e: any) => {

                        if (sender.state.value.length <= i) {
                            return;
                        }

                        var htmldetail = "<div class='distribution-list-user popover-detail bottom active'>";
                        htmldetail += "<span class='popover-detail-container'>";
                        htmldetail += "<span><label>Name :</label>" +
                            sender.state.value[i].firstname +
                            " " +
                            sender.state.value[i].lastname +
                            "</span>";
                        htmldetail += "<span><label>Job title :</label>" + sender.state.value[i].jobTitle + "</span>";
                        htmldetail += "<span><label>Department :</label>" +
                            sender.state.value[i].department +
                            "</span>";
                        htmldetail += "<span><label>Email :</label>" + sender.state.value[i].workEmail + "</span>";
                        htmldetail += "</span>";
                        htmldetail += "</div>";
                        $("body").append(htmldetail);
                        $("body").find(".distribution-list-user.active").each((i1: any, c1: any) => {
                            if (parseInt(i1) === 0) {
                                $("body").find(".distribution-list-user.active").eq(i1).css({
                                    left: (e.pageX + 10) + "px",
                                    top: (e.pageY + 10) + "px",
                                    "z-index": 900001
                                });
                            } else {
                                $("body").find(".distribution-list-user.active").eq(i1).remove();
                            }
                        });

                        e.stopPropagation();
                    });
                $(c).on("mouseout",
                    (e: any) => {
                        $("body").find(".distribution-list-user.active").remove();
                        e.stopPropagation();
                    });
            });
        }, 500);
    }

    onFilterChange = (event: any) => {
        //        const filter = event.filter.value;
        this.setState({
            sourceValue: filterBy(this.dataList, event.filter)
        });
    }

    onOpen(e: any) {
        if (window.navigator.userAgent.indexOf("Edge") > -1) {
            this.scrollPosition = $('html').scrollTop();
            $('html, body').animate({
                scrollTop: 0
            }, 0);
        }
    }

    onClose(e: any) {
        if (window.navigator.userAgent.indexOf("Edge") > -1) {
            $('html, body').animate({
                scrollTop: this.scrollPosition
            }, 0);
        }
    }

    render() {
        this.onBindTooltip();
        return (
            <div className="multiselect-wrapper">
                    <MultiSelect
                    onOpen={this.onOpen}
                    onClose={this.onClose}
                    data={this.state.sourceValue}
                    onChange={this.onChange}
                    value={this.state.value}
                    placeholder="Starting typing in firstname or lastname"
                    filterable={true}
                    onFilterChange={this.onFilterChange}
                    textField="displayName"
                    dataItemKey="userGuid"
                />
            </div>
        );
    }
}