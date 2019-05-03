import React, { Component } from "react";
import ContentLoader from "react-content-loader";
import { Link, RouteComponentProps } from "react-router-dom";
import { toast } from "react-toastify";
import { CompactAttachmentPreview } from "../../components/AttachmentPreview";
import Button from "../../components/Button";
import ConversationBox from "../../components/ConversationBox";
import FlexColumn from "../../components/FlexColumn";
import FlexRow from "../../components/FlexRow";
import SmartImg from "../../components/SmartImg";
import AgreementsService from "../../services/AgreementsService";
import { avatarUrl } from "../../services/ApiUtilities";
import {
  AgreementStatuses,
  getAgreementStatusText,
  IAgreementDetails,
} from "../../services/interfaces/AgreementInterfaces";
import { IBasicResponse } from "../../services/interfaces/Common";
import UserService from "../../services/UserService";
import { flatUrl, userProfileUrl } from "../../utilities/Utilities";
import Styles from "./AgreementDetails.module.css";

interface IAgreementDetailsRouteProps {
  id: string;
}

interface IAgreementDetailsState {
  agreement?: IAgreementDetails;
}

class AgreementDetails extends Component<RouteComponentProps<IAgreementDetailsRouteProps>, IAgreementDetailsState> {
  constructor(props: RouteComponentProps<IAgreementDetailsRouteProps>) {
    super(props);
    this.state = {};
    this.fetchAgreement(props.match.params.id);
  }

  public render() {
    const { agreement } = this.state;
    if (agreement === undefined) {
      return (
        <FlexColumn className={Styles.content}>
          <UserDataLoader />
        </FlexColumn>
      );
    }

    const actionButtons = [];
    if (agreement.owner.id === UserService.userId() && agreement.status.id === AgreementStatuses.Requested) {
      actionButtons.push(
        <Button className={Styles.accept} onClick={this.acceptAgreement}>
          Patvirtinti
        </Button>,
        <Button onClick={this.rejectAgreement}>Atmesti</Button>
      );
    } else if (agreement.tenant.id === UserService.userId() && agreement.status.id === AgreementStatuses.Requested) {
      actionButtons.push(<Button onClick={this.cancelAgreement}>Atšaukti</Button>);
    }

    return (
      <FlexColumn className={Styles.content}>
        <span className={Styles.agreementTitle}>Nuomos sutartis</span>
        <span className={Styles.flatNameHeader}>Nuomojamas butas:</span>
        <Link to={flatUrl(agreement.flat.id)} className={Styles.flatName}>
          {agreement.flat.name}
        </Link>
        <FlexRow className={Styles.detailsRow}>
          <FlexColumn className={Styles.users}>
            <Link className={Styles.noStyleLink} to={userProfileUrl(agreement.owner.id)}>
              <FlexRow className={Styles.user}>
                <div className={Styles.avatarWrapper}>
                  <SmartImg className={Styles.avatar} src={avatarUrl(agreement.owner.id)} />
                </div>
                <FlexColumn className={Styles.userDetails}>
                  <span className={Styles.userTitle}>Nuomotojas</span>
                  <span className={Styles.name}>
                    {agreement.owner.firstName} {agreement.owner.lastName}
                  </span>
                  <span className={Styles.name}>
                    {agreement.owner.phoneNumber} {agreement.owner.email}
                  </span>
                  {/* <span className={Styles.contact}>Siųsti žinutę</span> */}
                </FlexColumn>
              </FlexRow>
            </Link>
            <Link className={Styles.noStyleLink} to={userProfileUrl(agreement.tenant.id)}>
              <FlexRow className={Styles.user}>
                <div className={Styles.avatarWrapper}>
                  <SmartImg className={Styles.avatar} src={avatarUrl(agreement.tenant.id)} />
                </div>
                <FlexColumn className={Styles.userDetails}>
                  <span className={Styles.userTitle}>Nuomininkas</span>
                  <span className={Styles.name}>
                    {agreement.tenant.firstName} {agreement.tenant.lastName}
                  </span>
                  <span className={Styles.name}>
                    {agreement.tenant.phoneNumber} {agreement.tenant.email}
                  </span>
                  {/* <span className={Styles.contact}>Siųsti žinutę</span> */}
                </FlexColumn>
              </FlexRow>
            </Link>
          </FlexColumn>
          <FlexColumn className={Styles.agreementDetails}>
            <span className={Styles.detailHeader}>Nuomos laikotarpis:</span>
            <span className={Styles.detail}>Nuo {agreement.from.split("T")[0]}</span>
            <span className={Styles.detail}>Iki {agreement.to.split("T")[0]}</span>
            <span className={Styles.detailHeader}>Mėnesio kaina:</span>
            <span className={Styles.detail}>{agreement.price} Eur</span>
            <span className={Styles.detailHeader}>Statusas:</span>
            <span className={Styles.detail}>{getAgreementStatusText(agreement.status.id)}</span>
          </FlexColumn>
          {/* <Link to={conversationWithUserUrl(this.props.userId)} className={Styles.message}>
              Parašyti žinutę
            </Link> */}
        </FlexRow>
        <FlexColumn className={Styles.section}>
          <span className={Styles.sectionHeader}>Komentarai:</span>
          <span className={Styles.sectionText}>{agreement.comments}</span>
        </FlexColumn>
        <FlexColumn className={Styles.section}>
          <span className={Styles.sectionHeader}>Pridėti failai:</span>
          <CompactAttachmentPreview className={Styles.attachmentsBox} attachments={agreement.attachments} />
        </FlexColumn>
        <FlexRow className={Styles.buttons}>{actionButtons}</FlexRow>
        {/* Invoices */}
        {/* Faults */}
        <ConversationBox className={Styles.conversation} conversation={agreement.conversation} />
      </FlexColumn>
    );
  }

  private fetchAgreement = async (id: string) => {
    try {
      const { errors, data } = await AgreementsService.getDetails(id);
      if (errors !== undefined) {
        const error = Object.keys(errors).map((key) => errors![key].join("\n"));
        error.forEach((err) => toast.error(err));
      } else if (data !== undefined) {
        this.setState({ agreement: data });
      }
    } catch (error) {
      console.log(error);
      toast.error("Įvyko nežinoma klaida.");
    }
  };

  private cancelAgreement = async () => {
    this.updateAgreement(AgreementsService.cancelAgreement, () => {
      toast.success("Sutartis atšaukta!");
      this.props.history.push(userProfileUrl(UserService.userId()));
    });
  };

  private acceptAgreement = async () => {
    this.updateAgreement(AgreementsService.acceptAgreement, () => {
      toast.success("Sutartis patvirtinta!");
      this.fetchAgreement(this.props.match.params.id);
    });
  };

  private rejectAgreement = async () => {
    this.updateAgreement(AgreementsService.rejectAgreement, () => {
      toast.success("Sutartis atšaukta!");
      this.fetchAgreement(this.props.match.params.id);
    });
  };

  private updateAgreement = async (func: (id: string) => Promise<IBasicResponse>, onSuccess: () => void) => {
    try {
      if (this.state.agreement === undefined) {
        return;
      }
      const { errors } = await func(this.state.agreement.id);
      if (errors !== undefined) {
        const error = Object.keys(errors).map((key) => errors![key].join("\n"));
        error.forEach((err) => toast.error(err));
      } else {
        onSuccess();
      }
    } catch (error) {
      console.log(error);
      toast.error("Įvyko nežinoma klaida.");
    }
  };
}

export default AgreementDetails;

const UserDataLoader = () => (
  <ContentLoader height={130} width={380} speed={2} primaryColor="#f3f3f3" secondaryColor="#ecebeb">
    <rect x="70" y="15" rx="4" ry="4" width="117" height="6" />
    <rect x="70" y="35" rx="3" ry="3" width="85" height="6" />
    <rect x="0" y="80" rx="3" ry="3" width="350" height="6" />
    <rect x="0" y="100" rx="3" ry="3" width="380" height="6" />
    <rect x="0" y="120" rx="3" ry="3" width="201" height="6" />
    <circle cx="30" cy="30" r="30" />
  </ContentLoader>
);
