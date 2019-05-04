import React, { Component } from "react";
import ContentLoader from "react-content-loader";
import { Link, RouteComponentProps, Route } from "react-router-dom";
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
import InvoiceList from "./Invoice/InvoiceList";
import RoleRoute from "../../components/RoleRoute";
import { Authentication } from "../../Routes";
import InvoiceModal from "./Invoice/InvoiceModal";

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
          <TitleLoader />
          <UsersLoader />
          <CommentsLoader />
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

    const invoiceList = agreement.status.id === AgreementStatuses.Accepted
      ? <InvoiceList title="Sąskaitos" agreement={agreement} />
      : <></>;

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
                  <span className={Styles.userData}>
                    {agreement.owner.firstName} {agreement.owner.lastName}
                  </span>
                  <span className={Styles.userData}>
                    {agreement.owner.phoneNumber} {agreement.owner.email}
                  </span>
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
                  <span className={Styles.userData}>
                    {agreement.tenant.firstName} {agreement.tenant.lastName}
                  </span>
                  <span className={Styles.userData}>
                    {agreement.tenant.phoneNumber} {agreement.tenant.email}
                  </span>
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
        {invoiceList}
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

const UsersLoader = () => (
  <ContentLoader height={200} width={600} speed={2} primaryColor="#f3f3f3" secondaryColor="#ecebeb">
    <circle cx="40" cy="40" r="40" />
    <rect x="90" y="25" rx="4" ry="4" width="117" height="6" />
    <rect x="90" y="45" rx="3" ry="3" width="85" height="6" />
    <circle cx="40" cy="140" r="40" />
    <rect x="90" y="125" rx="4" ry="4" width="117" height="6" />
    <rect x="90" y="145" rx="3" ry="3" width="85" height="6" />

    <rect x="420" y="20" rx="3" ry="3" width="180" height="10" />
    <rect x="480" y="45" rx="3" ry="3" width="120" height="8" />
    <rect x="490" y="70" rx="3" ry="3" width="110" height="8" />

    <rect x="460" y="95" rx="3" ry="3" width="140" height="10" />
    <rect x="530" y="120" rx="3" ry="3" width="70" height="8" />
    <rect x="520" y="145" rx="3" ry="3" width="80" height="10" />
    <rect x="420" y="170" rx="3" ry="3" width="180" height="8" />
    {/* <rect x="490" y="55" rx="3" ry="3" width="110" height="6" /> */}
  </ContentLoader>
);

const TitleLoader = () => (
  <ContentLoader height={90} width={600} speed={2} primaryColor="#f3f3f3" secondaryColor="#ecebeb">
    <rect x="0" y="10" rx="3" ry="3" width="240" height="20" />
    <rect x="0" y="40" rx="3" ry="3" width="180" height="10" />
    <rect x="0" y="60" rx="3" ry="3" width="320" height="15" />
    {/* <rect x="490" y="55" rx="3" ry="3" width="110" height="6" /> */}
  </ContentLoader>
);

const CommentsLoader = () => (
  <ContentLoader height={140} width={600} speed={2} primaryColor="#f3f3f3" secondaryColor="#ecebeb">
    <rect x="0" y="10" rx="3" ry="3" width="110" height="8" />
    <rect x="0" y="30" rx="3" ry="3" width="540" height="6" />
    <rect x="0" y="50" rx="3" ry="3" width="520" height="6" />
    <rect x="0" y="70" rx="3" ry="3" width="430" height="6" />
    <rect x="0" y="90" rx="3" ry="3" width="570" height="6" />
    <rect x="0" y="110" rx="3" ry="3" width="490" height="6" />
    <rect x="0" y="130" rx="3" ry="3" width="360" height="6" />
    {/* <rect x="490" y="55" rx="3" ry="3" width="110" height="6" /> */}
  </ContentLoader>
);
