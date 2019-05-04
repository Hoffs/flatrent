import React, { Component } from "react";
import { RouteComponentProps } from "react-router-dom";
import { toast } from "react-toastify";
import Styles from "./InvoiceModal.module.css";

import Moment from "moment";
// tslint:disable-next-line: no-submodule-imports
import "moment/locale/lt";
import { DateRangePicker, FocusedInputShape } from "react-dates";
// tslint:disable-next-line: no-submodule-imports
import "react-dates/initialize";
// tslint:disable-next-line: no-submodule-imports
import "react-dates/lib/css/_datepicker.css";
import { IAgreementDetails } from "../../../services/interfaces/AgreementInterfaces";
import Dimmer from "../../../components/Dimmer";
import FlexColumn from "../../../components/FlexColumn";
import { stopPropogation, agreementUrl, joined } from "../../../utilities/Utilities";
import { IInvoiceDetails } from "../../../services/interfaces/InvoiceInterfaces";
import { IShortFaultDetails } from "../../../services/interfaces/FaultInterfaces";
import FlexRow from "../../../components/FlexRow";
import Button from "../../../components/Button";
import InvoiceService from "../../../services/InvoiceService";
import { IApiResponse, IErrorResponse } from "../../../services/interfaces/Common";
import UserService from "../../../services/UserService";
import { InputForm } from "../../../components/InputForm";

Moment.locale("lt");

export interface IInvoiceModalProps {
  agreement: IAgreementDetails;
  invoices: IInvoiceDetails[];
}

interface IInvoiceModalState {
  requesting: boolean;
  requestingPdf: boolean;
  errors: IErrorResponse;
}

class InvoiceModal extends Component<RouteComponentProps<{ id: string, invoiceId: string }> & IInvoiceModalProps, IInvoiceModalState> {
  constructor(props: RouteComponentProps<{ id: string, invoiceId: string }> & IInvoiceModalProps) {
    super(props);
    this.state = {
      requesting: false,
      requestingPdf: false,
      errors: {},
    };
  }

  public render() {
    const invoice = this.props.invoices.find(i => i.id === this.props.match.params.invoiceId);
    if (this.props.invoices.length > 0 && invoice === undefined) { this.exitModal(); return <></>; }
    if (invoice === undefined) { return <></>; }

    const dueDate = Moment.utc(invoice.dueDate);
    const todayDate = Moment.utc().startOf("day");

    const buttons = [];
    if (UserService.userId() === this.props.agreement.tenant.id && invoice.isValid && !invoice.isPaid) {
      buttons.push(
        <Button
          key={0}
          disabled={!invoice.isValid || this.state.requesting}
          onClick={this.payInvoiceFactory(invoice.id)}
          className={Styles.payButton}
        >
          Apmokėti
        </Button>,
      );
    }
    buttons.push(
      <Button
        disabled={this.state.requestingPdf}
        key={1}
        onClick={this.getPdfFactory(invoice.id)}
        className={Styles.pdfButton}
      >
        Gauti PDF
      </Button>,
      <Button key={2} onClick={this.exitModal} className={Styles.pdfButton}>
        Uždaryti
      </Button>
    );
    const daysLeft = dueDate.diff(todayDate, "days");
    const payUntilOrPaid = invoice.isPaid
      ? <span className={joined(Styles.subTitle, Styles.biggerFont)}>Sąskaita apmokėta</span>
      : <span className={joined(Styles.subTitle, Styles.biggerFont)}>
          Sumokėti iki {invoice.dueDate.split("T")[0]}, liko {daysLeft} d.
        </span>;

    return (
      <Dimmer onClick={this.exitModal}>
        <div className={Styles.modalWrapper}>
          <FlexColumn onClick={stopPropogation} className={Styles.modal}>
            <span className={Styles.title}>Sąskaita</span>
            <span className={Styles.subTitle}>Butas {this.props.agreement.flat.name}</span>
            <span className={joined(Styles.subTitle, Styles.biggerFont)}>
              Už laikotarpį nuo {invoice.invoicedPeriodFrom.split("T")[0]} iki{" "}
              {invoice.invoicedPeriodTo.split("T")[0]}
            </span>
            {payUntilOrPaid}
            <span className={Styles.incidentTitle}>Įskaičiuoti incidentai:</span>
            {this.getInvoiceIncidents(invoice.faults)}
            <span className={Styles.price}>Suma: {invoice.amountToPay} Eur</span>
            <InputForm className={Styles.errors} errorsOnly={true} errors={this.state.errors.general} />
            <FlexRow>
              {buttons}
            </FlexRow>
          </FlexColumn>
        </div>
      </Dimmer>
    );
  }

  private payInvoiceFactory = (invoiceId: string) => {
    return async () => {
      this.setState({ requesting: true });
      const response = await InvoiceService.payInvoice(this.props.agreement.id, invoiceId);
      if (response.errors !== undefined) {
        this.setState({ errors: response.errors, requesting: false });
      } else {
        this.setState({ requesting: false });
        const invoice = this.props.invoices.find(i => i.id === this.props.match.params.invoiceId);
        invoice!.isPaid = true;
        invoice!.isValid = false;
        invoice!.paidDate = Moment.utc().local().format("YYYY-MM-DD");
        toast.success("Sėkmingai apmokėta sąskaita!", {
          position: toast.POSITION.BOTTOM_CENTER,
        });
        this.exitModal();
      }
    }
  };

  private getPdfFactory = (invoiceId: string) => {
    return async () => {
      this.setState({ requestingPdf: true });
      const response = await InvoiceService.getPdf(this.props.agreement.id, invoiceId);
      console.log(response)
      if (response.errors !== undefined) {
        this.setState({ errors: response.errors, requestingPdf: false });
      } else {
        this.setState({ requesting: false });
        toast.success("Sėkmingai apmokėta sąskaita!", {
          position: toast.POSITION.BOTTOM_CENTER,
        });
        this.exitModal();
      }
    }
  };

  private getInvoiceIncidents = (faults: IShortFaultDetails[]) => {
    if (faults.length === 0) {
      return (
        <FlexRow key={0} className={Styles.incidentItem}>
          <span className={Styles.incidentName}>Nėra</span>
        </FlexRow>
      );
    }
    return faults.map((f, idx) => (
      <FlexRow key={f.id} className={Styles.incidentItem}>
        <span className={Styles.incidentName}>{idx + 1}. {f.name}</span>
        <span className={Styles.incidentPrice}>{f.price}</span>
      </FlexRow>
    ));
  };

  private exitModal = () => {
    this.props.history.push(agreementUrl(this.props.agreement.id));
  };
}

export default InvoiceModal;
