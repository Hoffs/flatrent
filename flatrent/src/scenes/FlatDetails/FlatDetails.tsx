import React, { Component } from "react";
import { RouteComponentProps, withRouter, Route, Link } from "react-router-dom";
import { toast } from "react-toastify";
import FlexRow from "../../components/FlexRow";
import FlatService, { IFlatDetails, IFlatDetailsResponse } from "../../services/FlatService";
import Styles from "./FlatDetails.module.css";

import ImageCarousel from "../../components/ImageCarousel";
import FlatDescription from "./FlatDescription";
import FlatShortInfo from "./FlatShortInfo";
import RentPanel from "./RentPanel";
import UserDisplay from "./UserInfo";
import RentModal from "./RentModal";
import FlexColumn from "../../components/FlexColumn";
import { flatEditUrl } from "../../utilities/Utilities";
import UserService from "../../services/UserService";

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

    const editNode = flat !== undefined && UserService.canEdit(flat.owner.id) ? (
      <Link className={Styles.editLink} to={flatEditUrl(flat.id)}>Redaguoti</Link>
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
                <UserDisplay user={flat ? flat.owner : undefined} />
              </FlexColumn>
            </FlexRow>

            <FlatDescription flat={flat} />
          </FlexRow>
          <RentPanel flat={flat} />
        </FlexRow>
        <Route exact={true} path={`${this.props.match.path}/rent`} component={RentModal} />
      </>
    );
  }

  private fetchFlat = (id: string) => {
    FlatService.getFlat(id)
      .then(this.handleResult)
      .catch(this.handleFail);
  }

  private handleResult = (result: IFlatDetailsResponse) => {
    if (result.errors !== undefined) {
      const errors = Object.keys(result.errors).map((key) => result.errors![key].join("\n"));
      errors.forEach((error) => toast.error(error));
    } else if (result.flat !== undefined) {
      this.setState({ flat: result.flat, loading: false });
    }
  }

  private handleFail() {
    toast.error("Įvyko nežinoma klaida.");
  }
}

export default withRouter(FlatDetails);
