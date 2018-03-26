using Microsoft.EntityFrameworkCore;
using PortableLibraryTelegramBot.Data.Database;
using System.Threading.Tasks;
using System.Linq;
using PortableLibraryTelegramBot.Data.Database.Entities;
using System;
using System.Collections.Generic;

namespace PortableLibraryTelegramBot.Services
{
    public class DatabaseService
    {
        private BotDataContext _context;

        public DatabaseService(BotDataContext context)
        {
            _context = context;
        }

        public async Task AddOrUpdateSequenceCommandAsync(long chatId, string command, string language, bool isComplete, string parentCommand = null)
        {
            var state = await _context.ChatCommandSequencesState.FirstOrDefaultAsync(s => s.ChatId == chatId && s.Command == command);
            if (state == null)
            {
                state = new ChatCommandSequenceState()
                {
                    ChatId = chatId,
                    Command = command,
                    Language = language,
                    CreationDate = DateTime.Now
                };

                if (!string.IsNullOrWhiteSpace(parentCommand))
                {
                    var parent = await _context.ChatCommandSequencesState.FirstOrDefaultAsync(s => s.ChatId == chatId && s.Command == parentCommand);
                    state.ParentChatCommandSequenceState = parent;
                }

                await _context.ChatCommandSequencesState.AddAsync(state);
            }

            state.IsComplete = isComplete;
        }

        public async Task<ChatCommandSequenceState> GetFirstCommand(long chatId)
        {
            var sequence = await _context.ChatCommandSequencesState.Where(s => s.ChatId == chatId)
                .OrderBy(s => s.CreationDate).FirstOrDefaultAsync();
            return sequence;
        }

        public async Task<ChatCommandSequenceState> GetLastCommand(long chatId)
        {
            var sequence = await _context.ChatCommandSequencesState.Where(s => s.ChatId == chatId)
                .OrderByDescending(s => s.CreationDate).FirstOrDefaultAsync();
            return sequence;
        }

        public async Task<List<ChatCommandSequenceState>> GetCommandSequence(long chatId)
        {
            var sequence = await _context.ChatCommandSequencesState.Where(s => s.ChatId == chatId)
                .OrderBy(s => s.CreationDate).ToListAsync();
            return sequence;
        }

        public async Task<string> GetCommandSequenceKey(long chatId)
        {
            var sequence = await GetCommandSequence(chatId);
            if (sequence.Count > 0)
                return string.Join(".", sequence.Select(s => s.Command));
            return null;
        }

        public async Task<bool> ClearSequence(long chatId)
        {
            var sequence = await GetCommandSequence(chatId);
            if (sequence.Count > 0 && sequence.All(s => s.IsComplete))
            {
                _context.ChatCommandSequencesState.RemoveRange(sequence.Where(s => s.ChatId == chatId));
                return true;
            }
            return false;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
