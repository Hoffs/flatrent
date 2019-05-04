import React, { Component, ReactNode } from "react";
import ContentLoader from "react-content-loader";
import { Link } from "react-router-dom";
import { toast } from "react-toastify";
import Button from "../../components/Button";
import FlexColumn from "../../components/FlexColumn";
import FlexRow from "../../components/FlexRow";
import { getAgreementStatusText, IShortAgreementDetails } from "../../services/interfaces/AgreementInterfaces";
import { IApiResponse } from "../../services/interfaces/Common";
import UserService from "../../services/UserService";
import { agreementUrl } from "../../utilities/Utilities";
import Styles from "./UserAgreements.module.css";

import "react-perfect-scrollbar/dist/css/styles.css";
import PerfectScrollbar from "react-perfect-scrollbar";

interface IUserAgreementsProps {
    title: string;
    userId: string;
    fetchData: (userId: string) => Promise<IApiResponse<IShortAgreementDetails[]>>;
}

interface IUserAgreementsState {
    agreements?: IShortAgreementDetails[];
}

class UserAgreements extends Component<IUserAgreementsProps, IUserAgreementsState> {
    constructor(props: IUserAgreementsProps) {
        super(props);
        this.state = {};
        this.fetchAgreements(props.userId);
    }

    public componentWillReceiveProps(newProps: IUserAgreementsProps) {
        this.setState({ agreements: undefined });
        this.fetchAgreements(newProps.userId);
    }

    public render() {
        const { agreements } = this.state;

        if (UserService.userId() !== this.props.userId) {
            return <></>;
        }

        return (
            <FlexColumn className={Styles.content}>
                <span className={Styles.title}>{this.props.title}</span>
                <PerfectScrollbar>{this.getAgreementItems(agreements)}</PerfectScrollbar>
            </FlexColumn>
        );
    }

    private getAgreementItems = (agreements?: IShortAgreementDetails[]): ReactNode[] => {
        if (agreements === undefined) {
            return Array(3)
                .fill(0)
                .map((_, idx) => <AgreementLoader key={idx} />);
        }

        return agreements.map((x) => (
            <FlexRow key={x.id} className={Styles.agreement}>
                <FlexColumn className={Styles.agreementContent}>
                    <FlexRow className={Styles.flatName}>{x.flatName}</FlexRow>
                    <FlexRow className={Styles.period}>
                        Nuo {x.from.split("T")[0]} iki {x.to.split("T")[0]}
                    </FlexRow>
                    <FlexRow className={Styles.status}>Statusas: {getAgreementStatusText(x.status.id)}</FlexRow>
                </FlexColumn>
                <Link className={Styles.moreLink} to={agreementUrl(x.id)}>
                    <Button className={Styles.moreButton}>Plačiau</Button>
                </Link>
            </FlexRow>
        ));
    };

    private fetchAgreements = async (userId: string) => {
        if (UserService.userId() !== userId) {
            return;
        }
        try {
            const { errors, data } = await this.props.fetchData(userId);
            if (errors !== undefined) {
                const error = Object.keys(errors).map((key) => errors![key].join("\n"));
                error.forEach((err) => toast.error(err));
            } else if (data !== undefined) {
                this.setState({ agreements: data });
            }
        } catch (error) {
            console.log(error);
            toast.error("Įvyko nežinoma klaida.");
        }
    };
}

export const AgreementLoader = () => (
    <div className={Styles.flatBox}>
        <div className={Styles.loader}>
            <ContentLoader height={100} width={480} speed={2} primaryColor="#f3f3f3" secondaryColor="#ecebeb">
                <rect x="2" y="20" rx="3" ry="3" width="320" height="12" />
                <rect x="2" y="44" rx="3" ry="3" width="220" height="8" />
                <rect x="2" y="64" rx="3" ry="3" width="190" height="8" />
                <rect x="370" y="30" rx="4" ry="4" width="100" height="40" />
            </ContentLoader>
        </div>
    </div>
);

export default UserAgreements;
