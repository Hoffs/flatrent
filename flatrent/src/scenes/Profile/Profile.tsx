import React, { Component } from "react";
import ContentLoader from "react-content-loader";
import { RouteComponentProps, withRouter } from "react-router-dom";
import FlexColumn from "../../components/FlexColumn";
import FlexRow from "../../components/FlexRow";
import UserService from "../../services/UserService";
import Styles from "./Profile.module.css";
import UserAgreements from "./UserAgreements";
import UserFlatList from "./UserFlatList";
import UserProfile from "./UserProfile";

class Profile extends Component<RouteComponentProps<{ id: string }>> {
  constructor(props: RouteComponentProps<{ id: string }>) {
    super(props);
  }

  public render() {
    const { id } = this.props.match.params;

    return (
      <FlexColumn className={Styles.content}>
        <UserProfile userId={id} />

        <FlexRow className={Styles.agreements}>
          <UserAgreements title="Nuomotojo sutartys" fetchData={UserService.getUserAgreementsOwner} userId={id} />
          <UserAgreements title="Nuomininko sutartys" fetchData={UserService.getUserAgreementsTenant} userId={id} />
        </FlexRow>
        <span className={Styles.flatsTitle}>Nuomojami butai</span>
        <UserFlatList userId={id} />
      </FlexColumn>
    );
  }

  public componentWillReceiveProps(newProps: RouteComponentProps<{ id: string }>) {
    if (this.props.match.params.id !== newProps.match.params.id) {
      this.forceUpdate();
    }
  }
}

export default withRouter(Profile);
