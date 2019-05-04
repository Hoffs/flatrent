import React, { Component, ReactNode } from "react";
import ContentLoader from "react-content-loader";
import { Link, RouteComponentProps } from "react-router-dom";
import { toast } from "react-toastify";
import Styles from "./InvoiceList.module.css";

import InfiniteScroll from "react-infinite-scroller";
import PerfectScrollbar from "react-perfect-scrollbar";
import "react-perfect-scrollbar/dist/css/styles.css";
import Button from "../../../components/Button";
import FlexColumn from "../../../components/FlexColumn";
import FlexRow from "../../../components/FlexRow";
import { IAgreementDetails } from "../../../services/interfaces/AgreementInterfaces";
import { IInvoiceDetails } from "../../../services/interfaces/InvoiceInterfaces";
import InvoiceService from "../../../services/InvoiceService";
import { invoiceUrl, joined } from "../../../utilities/Utilities";
import RoleRoute from "../../../components/RoleRoute";
import { Authentication } from "../../../Routes";
import InvoiceModal from "./InvoiceModal";

interface IInvoiceListProps {
    title: string;
    agreement: IAgreementDetails;
    className?: string;
}

interface IInvoiceListState {
    invoices: IInvoiceDetails[];
    hasMore: boolean;
}

class InvoiceList extends Component<IInvoiceListProps, IInvoiceListState> {
    constructor(props: IInvoiceListProps) {
        super(props);
        this.state = { invoices: [], hasMore: true };
    }

    public render() {
        const { invoices } = this.state;

        return (
            <FlexColumn className={joined(Styles.content, this.props.className)}>
                <span className={Styles.title}>{this.props.title}</span>
                <PerfectScrollbar>
                    <InfiniteScroll
                        className={Styles.scroller}
                        pageStart={0}
                        initialLoad={true}
                        loadMore={this.fetchInvoices}
                        hasMore={this.state.hasMore}
                        loader={<InvoiceLoader key={0} />}
                        useWindow={false}
                        isReverse={true}
                    >
                        {this.getItems(invoices)}
                    </InfiniteScroll>
                </PerfectScrollbar>
                <RoleRoute
                    redirect="/"
                    authenticated={Authentication.Authenticated}
                    path="/agreement/:id/invoice/:invoiceId"
                    render={this.getInvoiceModal}
                />
            </FlexColumn>
        );
    }

    private getInvoiceModal = (props: RouteComponentProps<any, any, any>) => (
        <InvoiceModal agreement={this.props.agreement} invoices={this.state.invoices} {...props} />
    );

    private getItems = (invoices?: IInvoiceDetails[]): ReactNode[] => {
        if (invoices === undefined) {
            return Array(3)
                .fill(0)
                .map((_, idx) => <InvoiceLoader key={idx} />);
        }

        return invoices.map((x) => (
            <FlexRow key={x.id} className={Styles.invoice}>
                <FlexColumn className={Styles.invoiceText}>
                    <span className={Styles.invoiceTitle}>
                        Sąskaita už laikotarpį nuo {x.invoicedPeriodFrom.split("T")[0]} iki{" "}
                        {x.invoicedPeriodTo.split("T")[0]}
                    </span>
                    <span className={Styles.invoiceData}>Apmokėti iki {x.dueDate.split("T")[0]}.</span>
                    <span className={Styles.invoiceStatus}>{this.getIsPaid(x)}</span>
                </FlexColumn>
                <Link className={Styles.moreLink} to={invoiceUrl(this.props.agreement.id, x.id)}>
                    <Button className={Styles.moreButton}>Plačiau</Button>
                </Link>
            </FlexRow>
        ));
    };

    private getIsPaid = (invoice: IInvoiceDetails) =>
        invoice.isPaid ? "Sąskaita apmokėta" : invoice.isValid ? "Sąskaita neapmokėta" : "Sąskaita nebegalioja";

    private fetchInvoices = async (_: number) => {
        try {
            const { errors, data } = await InvoiceService.getInvoices(
                this.props.agreement.id,
                this.state.invoices.length
            );
            if (errors !== undefined) {
                const error = Object.keys(errors).map((key) => errors![key].join("\n"));
                error.forEach((err) => toast.error(err));
            } else if (data !== undefined) {
                console.log(data);
                this.setState((state) => ({ invoices: [...state.invoices, ...data], hasMore: data.length === 16 }));
            }
        } catch (error) {
            console.log(error);
            toast.error("Įvyko nežinoma klaida.");
        }
    };
}

export const InvoiceLoader = () => (
    <div className={Styles.flatBox}>
        <div className={Styles.loader}>
            <ContentLoader height={80} width={600} speed={2} primaryColor="#f3f3f3" secondaryColor="#ecebeb">
                <rect x="2" y="20" rx="3" ry="3" width="320" height="12" />
                <rect x="2" y="37" rx="3" ry="3" width="220" height="8" />
                <rect x="2" y="50" rx="3" ry="3" width="190" height="8" />
                <rect x="490" y="20" rx="4" ry="4" width="110" height="40" />
            </ContentLoader>
        </div>
    </div>
);

export default InvoiceList;
