import { createMuiTheme } from "@material-ui/core";

export const materialTheme = createMuiTheme({
    typography: {
      useNextVariants: true,
    },
    overrides: {
        MuiOutlinedInput: {
            root: {
                '&$focused $notchedOutline': {
                  borderColor:"#004c75",
                  borderWidth: 2,
                },
                '& $notchedOutline': {
                  borderColor:"rgba(0, 76, 117, 0.1)",
                  borderWidth: 2,
                },
                '&:hover $notchedOutline': {
                  borderColor:"rgba(0, 76, 117, 0.4)",
                  borderWidth: 2,
                },
                "&:hover:not($disabled):not($focused):not($error) $notchedOutline": {
                  borderColor:"rgba(0, 76, 117, 0.4)",
                }
            },
            notchedOutline: {
                '&:hover': {
                    borderColor:"rgba(0, 76, 117, 0.4)",
                    borderWidth: 2,
                },
            }
        },
        MuiPickersToolbar: {
          toolbar: {
            backgroundColor: "#004c75",
          },
        },
        MuiPickersDay: {
          day: {
            color: "#5e8193",
            "&$selected": {
              backgroundColor: "#004c75",
            },
          },
          current: {
            color: "#004c75",
          },
        },
        MuiPickersModal: {
          dialogAction: {
            color: "#004c75",
          },
        },
      },
    });
