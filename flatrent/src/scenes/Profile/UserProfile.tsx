import React, { Component } from "react";
import ContentLoader from "react-content-loader";
import { Link } from "react-router-dom";
import { toast } from "react-toastify";
import FlexColumn from "../../components/FlexColumn";
import FlexRow from "../../components/FlexRow";
import SmartImg from "../../components/SmartImg";
import { avatarUrl } from "../../services/ApiUtilities";
import { IUserDetails } from "../../services/interfaces/UserInterfaces";
import UserService from "../../services/UserService";
import { conversationWithUserUrl, userProfileEditUrl } from "../../utilities/Utilities";
import Styles from "./UserProfile.module.css";

interface IUserProfileProps {
    userId: string;
}

interface IUserProfileState {
    user?: IUserDetails;
}

class UserProfile extends Component<IUserProfileProps, IUserProfileState> {
    constructor(props: IUserProfileProps) {
        super(props);
        this.state = {};
        this.fetchUserData(props.userId);
    }

    public componentWillReceiveProps(newProps: IUserProfileProps) {
        this.setState({ user: undefined });
        this.fetchUserData(newProps.userId);
    }

    public render() {
        const { user } = this.state;
        if (user === undefined) {
            return (
                <FlexColumn className={Styles.content}>
                    <UserDataLoader />
                </FlexColumn>
            );
        }

        const editNode =
            UserService.userId() === this.props.userId ? (
                <Link to={userProfileEditUrl(this.props.userId)} className={Styles.edit}>
                    Atnaujinti informaciją
                </Link>
            ) : (
                <></>
            );

        const sendMessageNode =
            UserService.userId() !== this.props.userId ? (
                <Link to={conversationWithUserUrl(this.props.userId)} className={Styles.message}>
                    Parašyti žinutę
                </Link>
            ) : (
                <></>
            );

        return (
            <FlexColumn className={Styles.content}>
                <FlexRow>
                    <FlexColumn className={Styles.avatarContent}>
                        <div className={Styles.avatarWrapper}>
                            <SmartImg className={Styles.avatar} src={avatarUrl(this.props.userId)} />
                        </div>
                    </FlexColumn>

                    <FlexColumn className={Styles.textContent}>
                        {editNode}
                        <span className={Styles.name}>
                            {user.firstName} {user.lastName}
                        </span>
                        <span className={Styles.joined}>
                            Prisijungė {user.createdDate.split("T")[0].split("-")[0]} metais
                        </span>
                        <span className={Styles.stats}>Nuomuotojo sutarčių: {user.ownerAgreementCount}</span>
                        <span className={Styles.stats}>Nuominiko sutarčių: {user.tenantAgreementCount}</span>
                        {sendMessageNode}
                    </FlexColumn>
                </FlexRow>
                <FlexColumn className={Styles.about}>
                    <span className={Styles.aboutHeader}>Apie mane:</span>
                    <span className={Styles.aboutText}>{user.about}</span>
                </FlexColumn>
            </FlexColumn>
        );
    }

    private fetchUserData = async (userId: string) => {
        try {
            const { errors, data } = await UserService.getUserData(userId);
            if (errors !== undefined) {
                const error = Object.keys(errors).map((key) => errors![key].join("\n"));
                error.forEach((err) => toast.error(err));
            } else if (data !== undefined) {
                this.setState({ user: data });
            }
        } catch (error) {
            console.log(error);
            toast.error("Įvyko nežinoma klaida.");
        }
    };
}

export default UserProfile;

const UserDataLoader = () => (
    <ContentLoader height={210} width={380} speed={2} primaryColor="#f3f3f3" secondaryColor="#ecebeb">
        <rect x="130" y="35" rx="4" ry="4" width="150" height="13" />
        <rect x="130" y="52" rx="3" ry="3" width="85" height="6" />
        <rect x="130" y="64" rx="3" ry="3" width="110" height="6" />
        <rect x="130" y="74" rx="3" ry="3" width="85" height="6" />
        <rect x="0" y="140" rx="3" ry="3" width="85" height="6" />
        <rect x="0" y="160" rx="3" ry="3" width="350" height="6" />
        <rect x="0" y="172" rx="3" ry="3" width="380" height="6" />
        <rect x="0" y="184" rx="3" ry="3" width="261" height="6" />
        <rect x="0" y="196" rx="3" ry="3" width="311" height="6" />
        <circle cx="60" cy="60" r="60" />
    </ContentLoader>
);
