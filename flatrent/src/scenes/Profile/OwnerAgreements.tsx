import React, { Component, ReactNode } from "react";
import ContentLoader from "react-content-loader";
import { toast } from "react-toastify";
import UserService from "../../services/UserService";
import Styles from "./OwnerAgreements.module.css";
import FlexColumn from "../../components/FlexColumn";
import FlexRow from "../../components/FlexRow";
import { IShortAgreementData } from "../../services/interfaces/AgreementInterfaces";

interface IOwnerAgreementsProps {
  userId: string;
}

interface IOwnerAgreementsState {
  agreements?: IShortAgreementData[];
}

class OwnerAgreements extends Component<IOwnerAgreementsProps, IOwnerAgreementsState> {
  constructor(props: IOwnerAgreementsProps) {
    super(props);
    this.state = {};
    this.fetchAgreements(props.userId);
  }

  componentWillReceiveProps(newProps: IOwnerAgreementsProps) {
    this.fetchAgreements(newProps.userId);
  }

  public render() {
    const { agreements } = this.state;
    if (UserService.userId() !== this.props.userId) {
      return (<></>);
    }

    if (agreements === undefined) {
      return (<div>loadin</div>);
    }

    return (
    <FlexColumn className={Styles.content}>
      <span className={Styles.title}>Nuomotojo sutartys</span>
      {this.getAgreementItems(agreements)}
    </FlexColumn>);
  }

  private getAgreementItems = (agreements: IShortAgreementData[]): ReactNode[] => {
    return agreements.map(x => (
      <FlexColumn key={x.id} className={Styles.agreement}>
        <FlexRow >{x.flatName}</FlexRow>
        <FlexRow>Nuo {x.from} iki {x.to}</FlexRow>
      </FlexColumn>
    ));
  }

  private fetchAgreements = async (userId: string) => {
    if (UserService.userId() !== userId) {
      return;
    }
    try {
      const {errors, data} = await UserService.getUserAgreementsOwner(userId);
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
  }
}

// const UserAgreementsA = ({ userId }: IUserAgreementsProps) => {
//   return (
//     <FlexColumn className={Styles.flatBox}>
//       <div className={Styles.imageWrapper}>
//         <Link className={Styles.imageLink} to={flatUrl(flat.id)} />
//         <SmartImg className={Styles.image} src={getImageUrl(flat.imageId)} />
//       </div>
//       <Link className={Styles.link} to={flatUrl(flat.id)}>
//         <FlexColumn className={Styles.content}>
//           <span className={Styles.address}>
//             {`${flat.address.street} • ${flat.address.city} • ${flat.address.country}`}
//           </span>
//           <span className={Styles.details}>
//             {`${flat.floor} aukštas • ${flat.area} m² • ${flat.roomCount} ${roomOrRooms(flat.roomCount)}  • ${flat.price} Eur`}
//           </span>
//           <span className={Styles.title}>{flat.name}</span>
//         </FlexColumn>
//       </Link>
//     </FlexColumn>
//   );
// };

export const AgreementLoader = () => (
  <div className={Styles.flatBox}>
    <div className={Styles.loader} >
      <ContentLoader
        speed={2}
        height={334}
        width={400}
        primaryColor="#f3f3f3"
        secondaryColor="#ecebeb"
      >
        <rect x="0" y="0" rx="5" ry="5" width="400" height="266" />
        <rect x="0" y="276" rx="4" ry="4" width="240" height="12" />
        <rect x="0" y="296" rx="4" ry="4" width="290" height="12" />
        <rect x="0" y="316" rx="4" ry="4" width="400" height="18" />
      </ContentLoader>
    </div>
  </div>
);
export default OwnerAgreements;
