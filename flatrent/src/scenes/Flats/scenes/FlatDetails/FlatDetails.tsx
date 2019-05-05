import React, { Component } from "react";
import { Link, Route, RouteComponentProps, withRouter } from "react-router-dom";
import { toast } from "react-toastify";
import FlexRow from "../../../../components/FlexRow";
import Styles from "./FlatDetails.module.css";

import FlexColumn from "../../../../components/FlexColumn";
import ImageCarousel from "../../../../components/ImageCarousel";
import RoleRoute from "../../../../components/RoleRoute";
import { Authentication } from "../../../../Routes";
import FlatService from "../../../../services/FlatService";
import { IApiResponse } from "../../../../services/interfaces/Common";
import { IFlatDetails } from "../../../../services/interfaces/FlatServiceInterfaces";
import UserService from "../../../../services/UserService";
import { flatEditUrl, flatRentUrl, loginUrl, userProfileUrl } from "../../../../utilities/Utilities";
import FlatDescription from "./FlatDescription";
import FlatShortInfo from "./FlatShortInfo";
import RentModal from "./RentModal";
import RentPanel from "./RentPanel";
import UserBox from "./UserBox";

interface IFlatDetailsState {
    loading: boolean;
    flat?: IFlatDetails;
}

class FlatDetails extends Component<RouteComponentProps<{ id: string }>, IFlatDetailsState> {
    constructor(props: RouteComponentProps<{ id: string }>) {
        super(props);
        this.state = {
            flat: undefined,
            loading: true,
        };
        this.fetchFlat(props.match.params.id);
    }

    public render() {
        const { flat } = this.state;

        const editNode =
            flat !== undefined && UserService.canEdit(flat.owner.id) ? (
                <FlexRow className={Styles.editActions}>
                    <Link className={Styles.editLink} to={flatEditUrl(flat.id)}>
                        Redaguoti
                    </Link>
                    <a className={Styles.deleteLink} onClick={this.deleteFlat}>
                        Ištrinti
                    </a>
                </FlexRow>
            ) : (
                <></>
            );

        return (
            <>
                <ImageCarousel
                    wrapperClassName={Styles.imageWrapper}
                    imageIds={flat === undefined ? undefined : flat.images.map((fi) => fi.id)}
                />
                <FlexRow className={Styles.contentWrapper}>
                    <FlexRow className={Styles.detailsContainer}>
                        {editNode}
                        <FlexRow className={Styles.sectionEnd}>
                            <FlatShortInfo flat={flat} />
                            <FlexColumn>
                                <UserBox user={flat ? flat.owner : undefined} />
                            </FlexColumn>
                        </FlexRow>

                        <FlatDescription flat={flat} />
                    </FlexRow>
                    <RentPanel flat={flat} />
                </FlexRow>
                {this.getRentRoute()}
            </>
        );
    }

    private getRentRoute = () => {
        return this.state.flat !== undefined ? (
            <RoleRoute
                authenticated={Authentication.Authenticated}
                redirect={loginUrl()}
                exact={true}
                path={flatRentUrl(this.state.flat.id)}
                render={this.getRentModal}
            />
        ) : (
            <></>
        );
    }

    private getRentModal = (props: RouteComponentProps<any, any, any>) =>
        this.state.flat !== undefined ? <RentModal flat={this.state.flat} {...props} /> : <></>

    private deleteFlat = async () => {
        await FlatService.deleteFlat(this.props.match.params.id);
        this.props.history.push(userProfileUrl(UserService.userId()));
    }

    private fetchFlat = (id: string) => {
        FlatService.getFlat(id)
            .then(this.handleResult)
            .catch(this.handleFail);
    }

    private handleResult = (result: IApiResponse<IFlatDetails>) => {
        if (result.errors !== undefined) {
            const errors = Object.keys(result.errors).map((key) => result.errors![key].join("\n"));
            errors.forEach((error) => toast.error(error));
        } else if (result.data !== undefined) {
            this.setState({ flat: result.data, loading: false });
        }
    }

    private handleFail() {
        toast.error("Įvyko nežinoma klaida.");
    }
}

export default withRouter(FlatDetails);
