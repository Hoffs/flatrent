import React, { Component } from "react";
import { toast } from "react-toastify";
import Button from "../../components/Button";
import Card from "../../components/Card";
import FlexColumn from "../../components/FlexColumn";
import FlexRow from "../../components/FlexRow";
import AgreementsService from "../../services/AgreementsService";
import { getAddressString } from "../../services/FlatService";
import UserService, { IAgreementData, IUserAgreements, IUserData } from "../../services/UserService";
import Styles from "./Profile.module.css";
import { IErrorResponse } from "../../services/interfaces/Common";

const doNothing = () => {};

interface IProfileState {
  user?: IUserData;
  ownerAgreements?: IAgreementData[];
  tenantAgreements?: IAgreementData[];
}

class Profile extends Component<{}, IProfileState> {
  constructor(props: {}) {
    super(props);
    this.state = { user: undefined, ownerAgreements: undefined, tenantAgreements: undefined };
    this.fetchData();
  }

  public render() {
    return (
      <>
        <Card className={Styles.titleCard}>
          <FlexRow className={Styles.titleRow}>
            <span className={Styles.title}>Mano duomenys</span>
            <FlexRow>
              <Button onClick={doNothing} outline={true}>
                Redaguoti
              </Button>
              <Button onClick={doNothing} outline={true}>
                Trinti paskyrą
              </Button>
            </FlexRow>
          </FlexRow>
          {this.getUserJsx()}
        </Card>

        <Card className={Styles.agreementCard}>
          <span className={Styles.agreementTitle}>Sutartys</span>
          {this.getAgreementsJsx()}
        </Card>
      </>
    );
  }

  private getUserJsx = () => {
    const { user } = this.state;
    if (user === undefined) {
      return <></>;
    }
    return (
      <FlexColumn className={Styles.userData}>
        <span>
          {user.firstName} {user.lastName}
        </span>
        <span>
          {user.email}, {user.phoneNumber}
        </span>
      </FlexColumn>
    );
  }

  private getAgreementsJsx = () => {
    const { tenantAgreements } = this.state;
    if (tenantAgreements === undefined) {
      return <></>;
    }

    const getPdfFunction = (id: string) => {
      return () => this.getPdfFile(id);
    };

    const getCancelFunction = (id: string) => {
      return () => this.cancelAgreement(id);
    };

    return tenantAgreements.map((x) => (
      <FlexRow key={x.id} className={Styles.agreementRow}>
        <FlexColumn className={Styles.rowText}>
          <span className={Styles.topLine}>
            {x.flatName}, {getAddressString(x.flatAddress)}
          </span>
          <span className={Styles.bottomLine}>
            Laikotarpis: {x.from.split("T")[0]} -> {x.to.split("T")[0]}
          </span>
        </FlexColumn>
        <Button className={Styles.button} outline={true} onClick={getCancelFunction(x.id)}>
          Nutraukti
        </Button>
        <Button className={Styles.button} outline={true} onClick={getPdfFunction(x.id)}>
          Sutartis
        </Button>
      </FlexRow>
    ));
  }

  private getPdfFile = async (id: string) => {
    try {
      const response = await AgreementsService.getPdf(id);
      if (Object.keys(response).length > 0) {
        const errors = Object.keys(response).map((key) => response![key].join("\n"));
        errors.forEach((error) => toast.error(error));
        return;
      }
    } catch (error) {
      toast.error("Įvyko nežinoma klaida.");
    }
  }
  private cancelAgreement = async (id: string) => {
    try {
      const response = await AgreementsService.cancelAgreement(id);
      if (Object.keys(response).length > 0) {
        const errors = Object.keys(response).map((key) => response![key].join("\n"));
        errors.forEach((error) => toast.error(error));
        return;
      }
      this.fetchUserAgreements();
    } catch (error) {
      toast.error("Įvyko nežinoma klaida.");
    }
  }

  private handleFail(e: any) {
    toast.error("Įvyko nežinoma klaida.");
  }

  private fetchData = () => {
    this.fetchUserData();
    this.fetchUserAgreements();
  }

  private fetchUserData = async () => {
    try {
      const response = await UserService.getUserData();
      if (isOfType<IUserData>(response)) {
        this.setState({ user: response });
      } else if (response !== undefined) {
        console.log(response);
        const errors = Object.keys(response).map((key) => response![key].join("\n"));
        errors.forEach((error) => toast.error(error));
      }
    } catch (error) {
      console.log(error);
      toast.error("Įvyko nežinoma klaida.");
    }
  }

  private fetchUserAgreements = async () => {
    try {
      const response = await UserService.getUserAgreements();
      if (isOfType<IUserAgreements>(response)) {
        this.setState({ ownerAgreements: response.owner, tenantAgreements: response.tenant });
      } else if (response !== undefined) {
        const errors = Object.keys(response).map((key) => response![key].join("\n"));
        errors.forEach((error) => toast.error(error));
      }
    } catch (error) {
      console.log(error);
      toast.error("Įvyko nežinoma klaida.");
    }
  }
}

function isOfType<T>(ob: T | IErrorResponse): ob is T {
  return (ob as IErrorResponse).errors === undefined;
}

export default Profile;
