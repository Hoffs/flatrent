import React, { Component } from "react";
import { RouteComponentProps, withRouter } from "react-router-dom";
import { toast } from "react-toastify";
import Button from "../../components/Button";
import Card from "../../components/Card";
import FlexColumn from "../../components/FlexColumn";
import FlexRow from "../../components/FlexRow";
import AgreementsService from "../../services/AgreementsService";
import { getAddressString } from "../../services/FlatService";
import UserService from "../../services/UserService";
import Styles from "./Profile.module.css";
import OwnerAgreements from "./OwnerAgreements";
import ContentLoader from "react-content-loader";
import UserInfo from "../FlatDetails/UserInfo";
import UserProfile from "./UserProfile";
import UserAgreements from "./UserAgreements";
import UserFlatList from "./UserFlatList";

const doNothing = () => {};

class Profile extends Component<RouteComponentProps<{ id: string }>> {
  constructor(props: RouteComponentProps<{ id: string }>) {
    super(props);
    this.state = { user: undefined, ownerAgreements: undefined, tenantAgreements: undefined };
    // this.fetchData();
  }

  public render() {
    const { id } = this.props.match.params;

    return (
      <FlexColumn className={Styles.content}>
        <UserProfile userId={id} />

        <FlexRow>
          <UserAgreements title="Nuomotojo sutartys" fetchData={UserService.getUserAgreementsOwner} userId={id} />
          <UserAgreements title="Nuomininko sutartys" fetchData={UserService.getUserAgreementsTenant} userId={id} />
        </FlexRow>
        <span className={Styles.flatsTitle}>Nuomojami butai</span>
        <UserFlatList userId={id} />
      </FlexColumn>
    );
  }

  componentWillReceiveProps(newProps: RouteComponentProps<{ id: string }>) {
    if (this.props.match.params.id !== newProps.match.params.id) {
      this.forceUpdate();
    }
  }

  // private getAgreementsJsx = () => {
  //   const { tenantAgreements } = this.state;
  //   if (tenantAgreements === undefined) {
  //     return <></>;
  //   }

  //   const getPdfFunction = (id: string) => {
  //     return () => this.getPdfFile(id);
  //   };

  //   const getCancelFunction = (id: string) => {
  //     return () => this.cancelAgreement(id);
  //   };

  //   return tenantAgreements.map((x) => (
  //     <FlexRow key={x.id} className={Styles.agreementRow}>
  //       <FlexColumn className={Styles.rowText}>
  //         <span className={Styles.topLine}>
  //           {x.flatName}, {getAddressString(x.Address)}
  //         </span>
  //         <span className={Styles.bottomLine}>
  //           Laikotarpis: {x.from.split("T")[0]} -> {x.to.split("T")[0]}
  //         </span>
  //       </FlexColumn>
  //       <Button className={Styles.button} outline={true} onClick={getCancelFunction(x.id)}>
  //         Nutraukti
  //       </Button>
  //       <Button className={Styles.button} outline={true} onClick={getPdfFunction(x.id)}>
  //         Sutartis
  //       </Button>
  //     </FlexRow>
  //   ));
  // }

  // private getPdfFile = async (id: string) => {
  //   try {
  //     const response = await AgreementsService.getPdf(id);
  //     if (Object.keys(response).length > 0) {
  //       const errors = Object.keys(response).map((key) => response![key].join("\n"));
  //       errors.forEach((error) => toast.error(error));
  //       return;
  //     }
  //   } catch (error) {
  //     toast.error("Įvyko nežinoma klaida.");
  //   }
  // }

  // private cancelAgreement = async (id: string) => {
  //   try {
  //     const response = await AgreementsService.cancelAgreement(id);
  //     if (Object.keys(response).length > 0) {
  //       const errors = Object.keys(response).map((key) => response![key].join("\n"));
  //       errors.forEach((error) => toast.error(error));
  //       return;
  //     }
  //     // this.fetchUserAgreements(this.props.match.params.id);
  //   } catch (error) {
  //     toast.error("Įvyko nežinoma klaida.");
  //   }
  // }

  // private handleFail(e: any) {
  //   toast.error("Įvyko nežinoma klaida.");
  // }

  // private fetchData = () => {
  //   const { id } = this.props.match.params;
  // }

  // private fetchUserAgreements = async (userId: string) => {
  //   try {
  //     const {errors, data} = await UserService.getUserAgreementsTenant(userId);
  //     if (errors !== undefined) {
  //       const error = Object.keys(errors).map((key) => errors![key].join("\n"));
  //       error.forEach((err) => toast.error(err));
  //     } else if (data !== undefined) {
  //       this.setState({ ownerAgreements: data.ownerAgreements, tenantAgreements: data.tenantAgreements });
  //     }
  //   } catch (error) {
  //     console.log(error);
  //     toast.error("Įvyko nežinoma klaida.");
  //   }
  // }

  // private fetchUserFlats = async (userId: string) => {
  //   try {
  //     const {errors, data} = await UserService.getUserFlats(userId);
  //     if (errors !== undefined) {
  //       const error = Object.keys(errors).map((key) => errors![key].join("\n"));
  //       error.forEach((err) => toast.error(err));
  //     } else if (data !== undefined) {
  //       this.setState({ flats: data });
  //     }
  //   } catch (error) {
  //     console.log(error);
  //     toast.error("Įvyko nežinoma klaida.");
  //   }
  // }
}

const ProfileLoader = () => (
  <ContentLoader height={130} width={380} speed={2} primaryColor="#f3f3f3" secondaryColor="#ecebeb">
    <rect x="70" y="15" rx="4" ry="4" width="117" height="6" />
    <rect x="70" y="35" rx="3" ry="3" width="85" height="6" />
    <rect x="0" y="80" rx="3" ry="3" width="350" height="6" />
    <rect x="0" y="100" rx="3" ry="3" width="380" height="6" />
    <rect x="0" y="120" rx="3" ry="3" width="201" height="6" />
    <circle cx="30" cy="30" r="30" />
  </ContentLoader>
);

export default withRouter(Profile);
