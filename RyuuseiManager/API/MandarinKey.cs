using MandarinJuiceCore;
using MandarinJuiceCore.GameProfile;
using MandarinJuiceCore.GamingPlatforms;
using MandarinJuiceCore.Helpers;
using MandarinJuiceCore.Infrastructure;
using Mi5hmasH.GameProfile;
using Mi5hmasH.Logger;
using Mi5hmasH.Logger.Models;
using Mi5hmasH.Logger.Providers;
using System;
using System.Collections.Generic;
using System.Text;

namespace RyuuseiManager.API
{
    public class MandarinKey
    {
        // Specific Mandarin Key of MMSFLC
        public const string key = "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";

        IGamingPlatform gamingPlatform;
        Core core = new Core(null, null);

        public byte[] DecryptBlob(byte[] encBlob, ulong steamId64)
        {
            LoadGameProfile();
            // Set User ID
            gamingPlatform.UserIdInput = steamId64.ToString();
            // Encrypt Files
            var encblob = core.DecryptBlob(encBlob, gamingPlatform);
            return encblob;
        }

        public byte[] EncryptBlob(byte[] decBlob, ulong steamId64)
        {
            LoadGameProfile();
            // Set User ID
            gamingPlatform.UserIdOutput = steamId64.ToString();
            // Encrypt Files
            var encblob = core.EncryptBlob(decBlob, gamingPlatform);
            return encblob;
        }

        public bool IsEncryptedBlob(byte[] blob, ulong steamId64)
        {
            LoadGameProfile();
            // Set User ID
            gamingPlatform.UserIdInput = steamId64.ToString();
            // Encrypt Files
            return core.IsEncrypted(blob, gamingPlatform);
        }

        private void LoadGameProfile()
        {
            var gameProfileManager = new GameProfileManager<MandarinGameProfile>();
            gameProfileManager.SetEncryptor(Keychain.GpMagic);
            gameProfileManager.Load(key, "SFLC");
            // Copy MandarinFileFlavor to Core
            core.MandarinFileFlavor = gameProfileManager.GameProfile.MandarinFileFlavor;
            // Configure Deencryptor
            core.Deencryptor.MandarinSeed = gameProfileManager.GameProfile.MandarinSeed;
            // Set GamingPlatform
            gamingPlatform = GamingPlatformHelper.GetGamingPlatform(gameProfileManager.GameProfile.Platform);
            gamingPlatform.ParseVariant = gameProfileManager.GameProfile.ParseVariant;
        }
    }
}
