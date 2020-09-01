"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react");
class Pagination extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            pageLimit: 5,
            totalPage: Math.ceil(this.props.totalCount / this.props.pageSize),
            pageInfo: ""
        };
    }
    render() {
        let pageNumbers = [];
        let totalPages = Math.ceil(this.props.totalCount / this.props.pageSize);
        let startPage, endPage;
        if (totalPages <= 10) {
            startPage = 1;
            endPage = totalPages;
        }
        else {
            if (this.props.activePage <= 6) {
                startPage = 1;
                endPage = 10;
            }
            else if (this.props.activePage + 4 >= totalPages) {
                startPage = totalPages - 9;
                endPage = totalPages;
            }
            else {
                startPage = this.props.activePage - 5;
                endPage = this.props.activePage + 4;
            }
        }
        let startIndex = (this.props.activePage - 1) * this.props.pageSize;
        let endIndex = Math.min(startIndex + this.props.pageSize - 1, this.props.totalCount - 1);
        for (let i = startPage; i <= endPage; i++) {
            pageNumbers.push(i);
        }
        const renderPageList = pageNumbers.map(number => {
            return (React.createElement("li", { key: number },
                React.createElement("a", { href: "javascript:void(0)", className: this.props.activePage === number ? 'k-state-selected' : 'k-link', onClick: () => this.props.onPageChange(number, ((number - 1) * this.props.pageSize)) }, number)));
        });
        return (React.createElement("div", { className: "k-pager-wrap k-grid-pager k-widget" },
            React.createElement("a", { href: "javascript:void(0);", className: this.props.activePage === 1 ? 'k-link k-pager-nav k-pager-first k-state-disabled' : 'k-link k-pager-nav k-pager-first', onClick: () => this.props.onPageChange(1, (this.props.pageSize)), title: "Go to the first page" },
                React.createElement("span", { className: "k-icon k-i-seek-w", "aria-label": "Go to the first page" })),
            React.createElement("a", { href: "#", className: this.props.activePage === 1 ? 'k-link k-pager-nav k-state-disabled' : 'k-link k-pager-nav', onClick: () => this.props.onPageChange(this.props.activePage - 1, ((this.props.activePage - 2) * this.props.pageSize)), title: "Go to the previous page" },
                React.createElement("span", { className: "k-icon k-i-arrow-w", "aria-label": "Go to the previous page" })),
            React.createElement("ul", { className: "k-pager-numbers k-reset" }, renderPageList),
            React.createElement("a", { href: "#", className: this.props.activePage === this.state.totalPage ? 'k-link k-pager-nav k-pager-last k-state-disabled' : 'k-link k-pager-nav k-pager-last', onClick: () => this.props.onPageChange(this.props.activePage + 1, (this.props.activePage * this.props.pageSize)), title: "Go to the next page" },
                React.createElement("span", { className: "k-icon k-i-arrow-e", "aria-label": "Go to the next page" })),
            React.createElement("a", { href: "#", className: this.props.activePage === this.state.totalPage ? 'k-link k-pager-nav k-pager-last k-state-disabled' : 'k-link k-pager-nav k-pager-last', onClick: () => this.props.onPageChange(Math.ceil(this.props.totalCount / this.props.pageSize), this.props.pageSize), title: "Go to the last page" },
                React.createElement("span", { className: "k-icon k-i-seek-e", "aria-label": "Go to the last page" })),
            React.createElement("label", { className: "k-pager-sizes k-label" },
                React.createElement("select", { onChange: this.props.onPageSizeChange },
                    React.createElement("option", { value: "12" }, " 12 "),
                    React.createElement("option", { value: "24" }, " 24 "),
                    React.createElement("option", { value: "36" }, " 36 "),
                    React.createElement("option", { value: "1000" }, " All ")),
                "items per page"),
            this.props.pageSize !== 1000 && React.createElement("div", { className: "k-pager-info k-label" },
                (this.props.activePage - 1) * this.props.pageSize + 1,
                " - ",
                ((this.props.activePage - 1) * this.props.pageSize) + this.props.pageSize,
                " of ",
                this.props.totalCount,
                " items"),
            this.props.pageSize === 1000 && React.createElement("div", { className: "k-pager-info k-label" },
                "Total ",
                this.props.totalCount,
                " items")));
    }
}
exports.Pagination = Pagination;
//# sourceMappingURL=Pagination.js.map