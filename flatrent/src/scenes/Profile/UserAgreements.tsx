import React, { Component, ReactNode } from "react";
import ContentLoader from "react-content-loader";
import { Link } from "react-router-dom";
import { toast } from "react-toastify";
import Button from "../../components/Button";
import FlexColumn from "../../components/FlexColumn";
import FlexRow from "../../components/FlexRow";
import { getAgreementStatusText, IShortAgreementData } from "../../services/interfaces/AgreementInterfaces";
import { IApiResponse } from "../../services/interfaces/Common";
import UserService from "../../services/UserService";
import { agreementUrl } from "../../utilities/Utilities";
import Styles from "./UserAgreements.module.css";

interface IUserAgreementsProps {
  title: string;
  userId: string;
  fetchData: (userId: string) => Promise<IApiResponse<IShortAgreementData[]>>;
}

interface IUserAgreementsState {
  agreements?: IShortAgreementData[];
}

class UserAgreements extends Component<IUserAgreementsProps, IUserAgreementsState> {
  constructor(props: IUserAgreementsProps) {
    super(props);
    this.state = {};
    this.fetchAgreements(props.userId);
  }

  public componentWillReceiveProps(newProps: IUserAgreementsProps) {
    this.fetchAgreements(newProps.userId);
  }

  public render() {
    const { agreements } = this.state;

    if (UserService.userId() !== this.props.userId) {
      return <></>;
    }

    if (agreements === undefined) {
      return <div>loadin</div>;
    }

    return (
      <FlexColumn className={Styles.content}>
        <span className={Styles.title}>{this.props.title}</span>
        {this.getAgreementItems(agreements)}
      </FlexColumn>
    );
  }

  private getAgreementItems = (agreements: IShortAgreementData[]): ReactNode[] => {
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
      <ContentLoader speed={2} height={334} width={400} primaryColor="#f3f3f3" secondaryColor="#ecebeb">
        <rect x="0" y="0" rx="5" ry="5" width="400" height="266" />
        <rect x="0" y="276" rx="4" ry="4" width="240" height="12" />
        <rect x="0" y="296" rx="4" ry="4" width="290" height="12" />
        <rect x="0" y="316" rx="4" ry="4" width="400" height="18" />
      </ContentLoader>
    </div>
  </div>
);

export default UserAgreements;
