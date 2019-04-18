export const getImageUrl = (id: string): string => {
    return `/api/image/${id}`;
};

export const getAvatarUrl = (id: string): string => {
    return `/api/user/${id}/avatar`;
};
