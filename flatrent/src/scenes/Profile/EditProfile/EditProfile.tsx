import React, { Component } from "react";
import { RouteComponentProps } from "react-router-dom";
import { toast } from "react-toastify";
import Styles from "./EditProfile.module.css";
import { IUpdateUserRequest, IUserDetails } from "../../../services/interfaces/UserInterfaces";
import FlexColumn from "../../../components/FlexColumn";
import FlexRow from "../../../components/FlexRow";
import { InputForm, InputAreaForm } from "../../../components/InputForm";
import Button from "../../../components/Button";
import UserService from "../../../services/UserService";
import { userProfileUrl } from "../../../utilities/Utilities";
import { CompactDropzone } from "../../../components/Dropzones";

interface IEditProfileState {
    values: {
        [key: string]: string;
    } & IUpdateUserRequest;
    requesting: boolean;
    files: File[],
    oldData?: IUserDetails,
    errors: { [key: string]: string[] };
}

class EditProfile extends Component<RouteComponentProps<{ id: string }>, IEditProfileState> {
    constructor(props: RouteComponentProps<{ id: string }>) {
        super(props);
        this.state = {
            errors: {},
            requesting: true,
            files: [],
            values: {
                about: "",
                bankAccount: "",
                phoneNumber: "",
                password: "",
                passwordConfirm: "",
            },
        };
        this.fetchUser();
    }

    public render() {
        return (
            <FlexColumn className={Styles.content}>
                <span className={Styles.title}>Atnaujinti informaciją</span>
                <FlexRow>
                    <InputForm
                        value={this.state.values.phoneNumber}
                        errors={this.state.errors.phoneNumber}
                        name="phoneNumber"
                        title="Tel. Numeris"
                        setValue={this.handleUpdate}
                    />
                </FlexRow>
                <InputForm
                    value={this.state.values.bankAccount}
                    errors={this.state.errors.bankAccount}
                    name="bankAccount"
                    title="Banko saskaita"
                    setValue={this.handleUpdate}
                />
                <InputAreaForm
                    className={Styles.about}
                    errors={this.state.errors.about}
                    name="about"
                    title="Apie"
                    value={this.state.values.about}
                    setValue={this.handleUpdate}
                    maxChars={1000}
                />
                <CompactDropzone
                    onDrop={this.updateAvatar}
                    files={this.state.files}
                    maxFiles={1}
                    accept={["image/png", "image/jpg", "image/jpeg"]}
                    text="Profilio nuotrauka.
                    Leistini formatai: png, jpg, jpeg.
                    Maksimalus nuotraukos dydis: 1 MB."
                    maxSize={1000000}
                />
                <FlexRow className={Styles.buttonRow}>
                    <Button disabled={this.state.requesting} onClick={this.updateUser}>
                        Atnaujinti
                    </Button>
                    <Button disabled={this.state.requesting} to={userProfileUrl(this.props.match.params.id)}>
                        Atšaukti
                    </Button>
                </FlexRow>
            </FlexColumn>
        );
    }

    private updateAvatar = (files: File[]) => this.setState({ files });

    private fetchUser = async () => {
        const result = await UserService.getUserData(this.props.match.params.id);
        if (result.data !== undefined) {
            this.setState({
                values: {
                    about: result.data.about !== undefined ? result.data.about : "",
                    bankAccount: result.data.bankAccount !== undefined ? result.data.bankAccount : "",
                    phoneNumber: result.data.phoneNumber !== undefined ? result.data.phoneNumber : "",
                },
                oldData: result.data,
                requesting: false,
            });
        } else {
            toast.error("Įvyko nežinoma klaida!", {
                position: toast.POSITION.BOTTOM_CENTER,
            });
            this.props.history.push(userProfileUrl(this.props.match.params.id));
        }
    };

    private updateUser = async () => {
        const { password, passwordConfirm, email, emailConfirm } = this.state.values;
        if (this.state.oldData === undefined) { return; }
        const errors: { [key: string]: string[] } = {};
        // if (password !== passwordConfirm) {
        //     errors.password = ["Slaptažodžiai turi būti vienodi."];
        // }
        if (Object.keys(errors).length > 0) {
            this.setState({ errors });
            return;
        }

        this.setState({ requesting: true });
        if (this.state.files.length > 0) {
            try {
                const result = await UserService.updateAvatar(this.props.match.params.id, this.state.files[0]);
                if (result.errors === undefined) {
                    toast.success("Sėkmingai atnaujinta profilio nuotrauka!", {
                        position: toast.POSITION.BOTTOM_CENTER,
                    });
                } else if (result.errors !== undefined) {
                    this.setState({ errors: result.errors });
                }
            } catch {
                // General error
            }
        }

        if (this.state.oldData.about === this.state.values.about
            && this.state.oldData.phoneNumber === this.state.values.phoneNumber
            && this.state.oldData.bankAccount === this.state.values.bankAccount) {
            this.props.history.push(userProfileUrl(this.props.match.params.id));
            return;
        }
        try {
            const result = await UserService.updateUser(this.props.match.params.id, this.state.values);
            if (result.errors === undefined) {
                toast.success("Sėkmingai atnaujinta informacija!", {
                    position: toast.POSITION.BOTTOM_CENTER,
                });
                this.props.history.push(userProfileUrl(this.props.match.params.id));
            } else if (result.errors !== undefined) {
                this.setState({ errors: result.errors });
            }
        } catch {
            // General error
        }
        this.setState({ requesting: false });
    };

    private handleUpdate = (name: string, value: string) =>
        this.setState((state) => ({ values: { ...state.values, [name]: value } }));
}

export default EditProfile;
