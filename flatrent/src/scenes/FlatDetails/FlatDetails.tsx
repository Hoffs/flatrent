import React, { Component } from "react";
import { Link, Route, RouteComponentProps, withRouter } from "react-router-dom";
import { toast } from "react-toastify";
import FlexRow from "../../components/FlexRow";
import Styles from "./FlatDetails.module.css";

import FlexColumn from "../../components/FlexColumn";
import ImageCarousel from "../../components/ImageCarousel";
import FlatService from "../../services/FlatService";
import { IFlatDetails } from "../../services/interfaces/FlatServiceInterfaces";
import UserService from "../../services/UserService";
import { flatEditUrl } from "../../utilities/Utilities";
import FlatDescription from "./FlatDescription";
import FlatShortInfo from "./FlatShortInfo";
import RentModal from "./RentModal";
import RentPanel from "./RentPanel";
import UserInfo from "./UserInfo";
import { IApiResponse } from "../../services/interfaces/Common";

interface IFlatDetailsState {
  loading: boolean;
  flat?: IFlatDetails;
}

class FlatDetails extends Component<RouteComponentProps<{ id: string }>, IFlatDetailsState> {
  constructor(props: RouteComponentProps<{ id: string }>) {
    super(props);
    this.state = {
      loading: true,
      flat: undefined,
    };
    this.fetchFlat(props.match.params.id);
  }

  public render() {
    const { flat } = this.state;

    const editNode =
      flat !== undefined && UserService.canEdit(flat.owner.id) ? (
        <Link className={Styles.editLink} to={flatEditUrl(flat.id)}>
          Redaguoti
        </Link>
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
                <UserInfo user={flat ? flat.owner : undefined} />
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
      <Route exact={true} path={`${this.props.match.path}/rent`} render={this.getRentModal} />
    ) : (
      <></>
    );
  }

  private getRentModal = (props: RouteComponentProps<any, any, any>) => (
    this.state.flat !== undefined ? <RentModal flat={this.state.flat} {...props} /> : <></>
  )

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
