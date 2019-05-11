import React, { Component, ReactNode } from "react";
import ContentLoader from "react-content-loader";
import { RouteComponentProps, Switch } from "react-router-dom";
import { toast } from "react-toastify";
import Styles from "./IncidentList.module.css";

import InfiniteScroll from "react-infinite-scroller";
import PerfectScrollbar from "react-perfect-scrollbar";
import "react-perfect-scrollbar/dist/css/styles.css";
import Button from "../../../components/Button";
import FlexColumn from "../../../components/FlexColumn";
import FlexRow from "../../../components/FlexRow";
import RoleRoute from "../../../components/RoleRoute";
import { Authentication } from "../../../Routes";
import IncidentService from "../../../services/IncidentService";
import { AgreementStatuses, IAgreementDetails } from "../../../services/interfaces/AgreementInterfaces";
import { IShortIncidentDetails } from "../../../services/interfaces/IncidentInterfaces";
import UserService from "../../../services/UserService";
import { incidentCreateUrl, incidentUrl, joined } from "../../../utilities/Utilities";
import CreateIncidentModal from "./CreateIncidentModal";
import ViewIncidentModal from "./ViewIncidentModal";

interface IIncidentListProps {
    title: string;
    agreement: IAgreementDetails;
    className: string;
}

interface IIncidentListState {
    incidents: IShortIncidentDetails[];
    hasMore: boolean;
}

class IncidentList extends Component<IIncidentListProps, IIncidentListState> {
    constructor(props: IIncidentListProps) {
        super(props);
        this.state = { incidents: [], hasMore: true };
    }

    public render() {
        const { incidents } = this.state;

        const statusId = this.props.agreement.status.id;
        const createButton =
            UserService.userId() === this.props.agreement.tenant.id && statusId === AgreementStatuses.Accepted ? (
                <Button to={incidentCreateUrl(this.props.agreement.id)}>Sukurti naują incidentą</Button>
            ) : (
                <></>
            );

        return (
            <FlexColumn className={joined(Styles.content, this.props.className)}>
                <span className={Styles.title}>{this.props.title}</span>
                <PerfectScrollbar>
                    <InfiniteScroll
                        className={Styles.scroller}
                        pageStart={0}
                        initialLoad={true}
                        loadMore={this.fetchIncidents}
                        hasMore={this.state.hasMore}
                        loader={<ItemLoader key={0} />}
                        useWindow={false}
                        isReverse={true}
                    >
                        {this.getItems(incidents)}
                    </InfiniteScroll>
                </PerfectScrollbar>
                {createButton}
                <Switch>
                    <RoleRoute
                        redirect="/"
                        authenticated={Authentication.Authenticated}
                        path="/agreement/:id/incident/new"
                        render={this.getCreateIncidentModal}
                    />
                    <RoleRoute
                        redirect="/"
                        authenticated={Authentication.Authenticated}
                        path="/agreement/:id/incident/:incidentId"
                        render={this.getViewIncidentModal}
                    />
                </Switch>
            </FlexColumn>
        );
    }

    private getCreateIncidentModal = (props: RouteComponentProps<any, any, any>) => (
        <CreateIncidentModal agreement={this.props.agreement} {...props} />
    )

    private getViewIncidentModal = (props: RouteComponentProps<any, any, any>) => (
        <ViewIncidentModal incidents={this.state.incidents} agreement={this.props.agreement} {...props} />
    )

    private getItems = (incidents?: IShortIncidentDetails[]): ReactNode[] => {
        if (incidents === undefined) {
            return Array(3)
                .fill(0)
                .map((_, idx) => <ItemLoader key={idx} />);
        }

        return incidents.map((x) => (
            <FlexRow key={x.id} className={Styles.item}>
                <FlexColumn>
                    <FlexRow className={Styles.itemTitle}>{x.name}</FlexRow>
                    <FlexRow className={Styles.itemData}>Statusas: {this.getIsRepaired(x)}</FlexRow>
                    <FlexRow className={Styles.itemStatus}>Kaina: {this.getPrice(x)}</FlexRow>
                </FlexColumn>
                <Button to={incidentUrl(this.props.agreement.id, x.id)} className={Styles.moreButton}>
                    Plačiau
                </Button>
            </FlexRow>
        ));
    }

    private getIsRepaired = (incident: IShortIncidentDetails) => (incident.repaired ? "Sutaisytas" : "Nesutaisytas");
    private getPrice = (incident: IShortIncidentDetails) => (incident.price === 0 ? "Nenustatyta" : incident.price);

    private fetchIncidents = async (_: number) => {
        try {
            const { errors, data } = await IncidentService.getIncidents(
                this.props.agreement.id,
                this.state.incidents.length,
            );
            if (errors !== undefined) {
                const error = Object.keys(errors).map((key) => errors![key].join("\n"));
                error.forEach((err) => toast.error(err));
            } else if (data !== undefined) {
                console.log(data);
                this.setState((state) => ({ incidents: [...state.incidents, ...data], hasMore: data.length === 16 }));
            }
        } catch (error) {
            console.log(error);
            toast.error("Įvyko nežinoma klaida.");
        }
    }
}

export const ItemLoader = () => (
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

export default IncidentList;
